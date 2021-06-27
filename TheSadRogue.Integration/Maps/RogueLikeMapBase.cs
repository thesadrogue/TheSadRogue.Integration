using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GoRogue.Components;
using GoRogue.FOV;
using GoRogue.GameFramework;
using GoRogue.Pathing;
using GoRogue.SpatialMaps;
using SadConsole;
using SadConsole.Components;
using SadConsole.Entities;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace SadRogue.Integration.Maps
{
    public struct DefaultRendererParams
    {
        public Point? ViewSize;
        public IFont? Font;
        public Point? FontSize;

        public DefaultRendererParams(Point? viewSize = null, IFont? font = null, Point? fontSize = null)
        {
            ViewSize = viewSize;
            Font = font;
            FontSize = fontSize;
        }

        public static implicit operator (Point? viewSize, IFont? font, Point? fontSize)(DefaultRendererParams obj)
            => (obj.ViewSize, obj.Font, obj.FontSize);

        public static implicit operator DefaultRendererParams((Point? viewSize, IFont? font, Point? fontSize) tuple)
            => new DefaultRendererParams(tuple.viewSize, tuple.font, tuple.fontSize);
    }

    /// <summary>
    /// Abstract base class for a Map that contains the necessary function to render to one or more ScreenSurfaces.
    /// </summary>
    public partial class RogueLikeMapBase : Map, IScreenObject
    {
        private readonly List<IScreenSurface> _renderers;
        private readonly Dictionary<IScreenSurface, Renderer> _surfaceEntityRenderers;

        /// <summary>
        /// List of renderers (IScreenSurfaces) that currently render the map.
        /// </summary>
        public IReadOnlyList<IScreenSurface> Renderers => _renderers.AsReadOnly();

        /// <summary>
        /// An IGridView of the ColoredGlyphs on the Terrain layer (0) that will produce a fully
        /// transparent appearance if there is no terrain object at the requested location.
        /// </summary>
        public readonly IGridView<ColoredGlyph> TerrainView;

        private static readonly ColoredGlyph _transparentAppearance =
            new ColoredGlyph(Color.Transparent, Color.Transparent, 0, Mirror.None);

        /// <summary>
        /// Each and every component attached to the map.
        /// </summary>
        /// <remarks>
        /// Confused about which collection to add a component to?
        /// Add it here.
        /// </remarks>
        public IComponentCollection AllComponents => GoRogueComponents;

        private IScreenSurface? _defaultRenderer;

        /// <summary>
        /// A render that is attached to the map as a child, and will render the contents of the map.  If it is null,
        /// no default renderer is created.
        /// </summary>
        public IScreenSurface? DefaultRenderer
        {
            get => _defaultRenderer;
            set
            {
                if (_defaultRenderer == value) return;

                if (value != null && !_renderers.Contains(value))
                    throw new InvalidOperationException(
                        $"Only surfaces created/configured with {nameof(CreateRenderer)} may be used as a map's {nameof(DefaultRenderer)}.");

                if (_defaultRenderer != null)
                {
                    Children.Remove(_defaultRenderer);
                    RemoveRenderer(_defaultRenderer);
                }

                _defaultRenderer = value;
                if (_defaultRenderer != null)
                    Children.Add(_defaultRenderer);
            }
        }

        /// <summary>
        /// Creates a new RogueLikeMap.
        /// </summary>
        /// <remarks>
        /// The map accepts a <paramref name="defaultRendererParams"/> parameter, which specifies font and viewport
        /// information to use for creating a ScreenSurface to render this map as it is created.  The surface will
        /// automatically be created and assigned to the <see cref="DefaultRenderer"/> property.  It will also be added
        /// as a child object of the map, and as such, the renderer will automatically display the map whenever the map
        /// itself is added as a screen to the SadConsole object hierarchy.
        ///
        /// If you wish to use a custom screen surface as the map renderer, you may specify "null" for the
        /// <paramref name="defaultRendererParams"/> parameter.  You may then assign an appropriate renderer to
        /// <see cref="DefaultRenderer"/> manually, in which case it will be added as a child object of the map, and
        /// its faculties will be called as appropriate when the map is added as a screen to the SadConsole hierarchy.
        ///
        /// Note that any renderer assigned to DefaultRenderer is expected to have been created by CreateRenderer.
        /// An overload is provided that allows for providing a function to create the instance used, so it is possible
        /// to use arbitrary IScreenSurface implementations with this function.
        ///
        /// Alternatively, you can elect to simply create renderers using the CreateRenderer functions and not assign
        /// use the <see cref="DefaultRenderer"/> field at all.  In this case, you must ensure that BOTH the map AND
        /// the renderer are independently added to the SadConsole screen hierarchy.
        /// </remarks>
        /// <param name="width">Width of map.</param>
        /// <param name="height">Height of the map.</param>
        /// <param name="defaultRendererParams">
        /// Parameters to use for creation of the default rendering surface.  If null is specified, renderer creation
        /// will need to be performed manually.
        /// </param>
        /// <param name="numberOfEntityLayers">How many entity (eg. non-terrain) layers to include.</param>
        /// <param name="distanceMeasurement">How to measure distance for pathing, movement, etc.</param>
        /// <param name="layersBlockingWalkability">Which layers should participate in collision detection.  Defaults to all layers.</param>
        /// <param name="layersBlockingTransparency">Which layers should participate in determining transparency of tiles.  Defaults to all layers.</param>
        /// <param name="entityLayersSupportingMultipleItems">Which entity layers support having multiple objects on the same square.  Defaults to all layers.</param>
        /// <param name="customPlayerFOV">
        /// Custom FOV to use for <see cref="Map.PlayerFOV"/>.  Defaults to GoRogue's recursive shadow-casting
        /// implementation.  It may also be useful to specify this if you want the <see cref="Map.PlayerFOV"/> property
        /// to not use <see cref="Map.TransparencyView"/> for data.
        /// </param>
        /// <param name="customPather">
        /// Custom A* pathfinder for the map.  Typically, you wont' need to specify this; By default, uses
        /// <see cref="Map.WalkabilityView"/> to determine which locations can be reached, and calculates distance based
        /// on the <see cref="Distance" /> passed in via the constructor.
        /// </param>
        /// <param name="customComponentContainer">
        /// A custom component container to use for <see cref="Map.GoRogueComponents"/>.  If not specified, a
        /// <see cref="ComponentCollection"/> is used.  Typically you will not need to specify this, as a
        /// ComponentCollection is sufficient for nearly all use cases.
        /// </param>
        public RogueLikeMapBase(int width, int height, DefaultRendererParams? defaultRendererParams, int numberOfEntityLayers,
                                   Distance distanceMeasurement, uint layersBlockingWalkability = uint.MaxValue,
                                   uint layersBlockingTransparency = uint.MaxValue,
                                   uint entityLayersSupportingMultipleItems = uint.MaxValue, IFOV? customPlayerFOV = null,
                                   AStar? customPather = null, IComponentCollection? customComponentContainer = null)
            : base(width, height, numberOfEntityLayers, distanceMeasurement, layersBlockingWalkability,
                   layersBlockingTransparency, entityLayersSupportingMultipleItems, customPlayerFOV, customPather,
                   customComponentContainer)
        {
            ObjectAdded += Object_Added;
            ObjectRemoved += Object_Removed;

            _renderers = new List<IScreenSurface>();
            _surfaceEntityRenderers = new Dictionary<IScreenSurface, Renderer>();
            TerrainView = new LambdaTranslationGridView<IGameObject?, ColoredGlyph>(Terrain, GetTerrainAppearance);

            AllComponents.ComponentAdded += On_GoRogueComponentAdded;
            AllComponents.ComponentRemoved += On_GoRogueComponentRemoved;

            // ScreenObject initialization
            UseMouse = Settings.DefaultScreenObjectUseMouse;
            UseKeyboard = Settings.DefaultScreenObjectUseKeyboard;
            SadComponents = new ObservableCollection<IComponent>();
            ComponentsUpdate = new List<IComponent>();
            ComponentsRender = new List<IComponent>();
            ComponentsKeyboard = new List<IComponent>();
            ComponentsMouse = new List<IComponent>();
            ComponentsEmpty = new List<IComponent>();
            SadComponents.CollectionChanged += Components_CollectionChanged;
            Children = new ScreenObjectCollection(this);

            // Create a default renderer if needed
            if (defaultRendererParams.HasValue)
                DefaultRenderer = CreateRenderer(defaultRendererParams.Value.ViewSize, defaultRendererParams.Value.Font,
                    defaultRendererParams.Value.FontSize);
        }

        /// <summary>
        /// Creates a ScreenSurface that renders this map.  When the surface is no longer used,
        /// <see cref="RemoveRenderer"/> must be called.
        /// </summary>
        /// <param name="viewSize">Viewport size for the renderer.</param>
        /// <param name="font">Font to use for the renderer.</param>
        /// <param name="fontSize">Size of font to use for the renderer.</param>
        /// <returns>A renderer configured with the given parameters.</returns>
        public ScreenSurface CreateRenderer(Point? viewSize = null, IFont? font = null, Point? fontSize = null)
            => (ScreenSurface)CreateRenderer(CreateDefaultScreenSurface, viewSize, font, fontSize);

        /// <summary>
        /// Use the given function to create an IScreenSurface, and configure it to render this map.  When the surface
        /// is no longer used, <see cref="RemoveRenderer"/> must be called.
        /// </summary>
        /// <remarks>
        /// This allows custom screen surface implementations (for example, ScreenSurface subclasses) to be used to
        /// render the map.  The creation function given is expected to create a screen surface which renders the
        /// cell surface specified, using the font specified.
        /// </remarks>
        /// <param name="surfaceCreator">Function that will create a new surface with the specified parameters.</param>
        /// <param name="viewSize">Viewport size for the renderer.</param>
        /// <param name="font">Font to use for the renderer.</param>
        /// <param name="fontSize">Size of font to use for the renderer.</param>
        /// <returns>A renderer created using the given function, configured with the given parameters.</returns>
        public IScreenSurface CreateRenderer(Func<ICellSurface, IFont?, Point?, IScreenSurface> surfaceCreator, Point? viewSize = null, IFont? font = null, Point? fontSize = null)
        {
            // Default view size is entire Map
            var (viewWidth, viewHeight) = viewSize ?? (Width, Height);

            // Create surface representing the terrain layer of the map
            var cellSurface = new MapTerrainCellSurface(this, viewWidth, viewHeight);

            // Create screen surface that renders that cell surface and keep track of it
            var renderer = surfaceCreator(cellSurface, font, fontSize);
            _renderers.Add(renderer);

            // Create an EntityRenderer and configure it with all the appropriate entities,
            // then add it to the main surface
            var entityRenderer = new Renderer { DoEntityUpdate = false };
            _surfaceEntityRenderers[renderer] = entityRenderer;
            // TODO: Reverse this order when it won't cause NullReferenceException
            renderer.SadComponents.Add(entityRenderer);
            entityRenderer.AddRange(Entities.Items.Cast<Entity>());

            // Return renderer
            return renderer;
        }

        /// <summary>
        /// Removes a renderer from the list of renders displaying the map.  This must be called when a renderer is no
        /// longer used, in order to ensure that the renderer resources are freed
        /// </summary>
        /// <param name="renderer">The renderer to unlink.</param>
        public void RemoveRenderer(IScreenSurface renderer)
        {
            _renderers.Remove(renderer);
            _surfaceEntityRenderers.Remove(renderer);
        }

        private void Object_Added(object? sender, ItemEventArgs<IGameObject> e)
        {
            switch (e.Item)
            {
                case RogueLikeCell terrain:
                    // Ensure we flag the surfaces of renderers as dirty on the add and on subsequent isDirty events
                    terrain.Appearance.IsDirtySet += Terrain_AppearanceIsDirtySet;
                    Terrain_AppearanceIsDirtySet(terrain.Appearance, EventArgs.Empty);
                    break;

                case RogueLikeEntity entity:
                    // Add to any entity renderers we have
                    foreach (var renderers in _surfaceEntityRenderers.Values)
                        renderers.Add(entity);

                    break;

                default:
                    throw new InvalidOperationException(
                        $"Objects added to a {GetType().Name} must be of type {nameof(RogueLikeCell)} for terrain, or {nameof(RogueLikeEntity)} for non-terrain");
            }
        }

        private void Object_Removed(object? sender, ItemEventArgs<IGameObject> e)
        {
            switch (e.Item)
            {
                case RogueLikeCell terrain:
                    // Ensure we flag the surfaces of renderers as dirty on the remove and unlike our changed handler
                    terrain.Appearance.IsDirtySet -= Terrain_AppearanceIsDirtySet;
                    Terrain_AppearanceIsDirtySet(terrain.Appearance, EventArgs.Empty);
                    break;

                case RogueLikeEntity entity:
                    // Remove from any entity renderers we have
                    foreach (var renderers in _surfaceEntityRenderers.Values)
                        renderers.Remove(entity);
                    break;
            }
        }

        private void On_GoRogueComponentAdded(object? s, ComponentChangedEventArgs e)
        {
            if (e.Component is IComponent sadComponent)
                SadComponents.Add(sadComponent);
        }

        private void On_GoRogueComponentRemoved(object? s, ComponentChangedEventArgs e)
        {
            if (e.Component is IComponent sadComponent)
                SadComponents.Remove(sadComponent);
        }

        private void Terrain_AppearanceIsDirtySet(object? sender, EventArgs e)
        {
            // Nullable override because IsDirtySet never sends null
            RogueLikeCell terrain = ((TerrainAppearance)sender!).Terrain;

            foreach (var renderer in _renderers)
                if (renderer.Surface.View.Contains(terrain.Position))
                    renderer.IsDirty = true;
        }

        private static ColoredGlyph GetTerrainAppearance(IGameObject? gameObject)
            => ((RogueLikeCell?) gameObject)?.Appearance ?? _transparentAppearance;

        private static IScreenSurface CreateDefaultScreenSurface(ICellSurface surface, IFont? font, Point? fontSize)
            => new ScreenSurface(surface, font, fontSize);
    }
}

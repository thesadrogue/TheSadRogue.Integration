using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.Components;
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
    /// <summary>
    /// Abstract base class for a Map that contains the necessary function to render to one or more ScreenSurfaces.
    /// </summary>
    public abstract partial class RogueLikeMapBase : Map, IScreenObject
    {
        private readonly List<ScreenSurface> _renderers;
        private readonly Dictionary<ScreenSurface, Renderer> _surfaceEntityRenderers;

        /// <summary>
        /// List of renderers (ScreenSurfaces) that currently render the map.
        /// </summary>
        public IReadOnlyList<ScreenSurface> Renderers => _renderers.AsReadOnly();

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

        /// <summary>
        /// Creates a new RogueLikeMapBase.
        /// </summary>
        /// <param name="backingObject">The object being used for the map's IScreenObject implementation.</param>
        /// <param name="width">Width of map.</param>
        /// <param name="height">Height of the map.</param>
        /// <param name="numberOfEntityLayers">How many entity (eg. non-terrain) layers to include.</param>
        /// <param name="distanceMeasurement">How to measure distance for pathing, movement, etc.</param>
        /// <param name="layersBlockingWalkability">Which layers should participate in collision detection.  Defaults to all layers.</param>
        /// <param name="layersBlockingTransparency">Which layers should participate in determining transparency of tiles.  Defaults to all layers.</param>
        /// <param name="entityLayersSupportingMultipleItems">Which entity layers support having multiple objects on the same square.  Defaults to all layers.</param>
        /// <param name="customPlayerFOV">
        /// Custom FOV to use for <see cref="Map.PlayerFOV"/>.  Typically you will not need to specify this; it is
        /// generally only useful if you want this property to NOT use <see cref="Map.TransparencyView"/> for data.
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
        protected RogueLikeMapBase(IScreenObject backingObject, int width, int height, int numberOfEntityLayers, Distance distanceMeasurement,
            uint layersBlockingWalkability = uint.MaxValue, uint layersBlockingTransparency = uint.MaxValue,
            uint entityLayersSupportingMultipleItems = uint.MaxValue, FOV? customPlayerFOV = null,
            AStar? customPather = null, IComponentCollection? customComponentContainer = null)
            : base(width, height, numberOfEntityLayers,
            distanceMeasurement, layersBlockingWalkability, layersBlockingTransparency,
            entityLayersSupportingMultipleItems, customPlayerFOV, customPather, customComponentContainer)
        {
            BackingObject = backingObject;

            ObjectAdded += Object_Added;
            ObjectRemoved += Object_Removed;

            _renderers = new List<ScreenSurface>();
            _surfaceEntityRenderers = new Dictionary<ScreenSurface, Renderer>();
            TerrainView = new LambdaTranslationGridView<IGameObject?, ColoredGlyph>(Terrain, GetTerrainAppearance);

            AllComponents.ComponentAdded += On_GoRogueComponentAdded;
            AllComponents.ComponentRemoved += On_GoRogueComponentRemoved;

        }

        /// <summary>
        /// Creates a renderer that renders this Map.  When no longer used, <see cref="DestroyRenderer"/> must
        /// be called.
        /// </summary>
        /// <param name="viewSize">Viewport size for the renderer.</param>
        /// <param name="font">Font to use for the renderer.</param>
        /// <param name="fontSize">Size of font to use for the renderer.</param>
        /// <returns>A renderer configured with the given parameters.</returns>
        protected ScreenSurface CreateRenderer(Point? viewSize = null, Font? font = null, Point? fontSize = null)
        {
            // Default view size is entire Map
            var (viewWidth, viewHeight) = viewSize ?? (Width, Height);

            // Create surface representing the terrain layer of the map
            var cellSurface = new MapTerrainCellSurface(this, viewWidth, viewHeight);

            // Create screen surface that renders that cell surface and keep track of it
            var renderer = new ScreenSurface(cellSurface, font, fontSize);
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
        protected void DestroyRenderer(ScreenSurface renderer)
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
    }
}

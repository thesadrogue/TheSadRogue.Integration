using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.Components;
using GoRogue.GameFramework;
using GoRogue.SpatialMaps;
using SadConsole;
using SadConsole.Components;
using SadConsole.Entities;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace TheSadRogue.Integration.Maps
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
        public ITaggableComponentCollection? AllComponents => GoRogueComponents;

        /// <summary>
        /// Creates a new RogueLikeMapBase.
        /// </summary>
        /// <param name="backingObject">The object being used for the map's IScreenObject implementation.</param>
        /// <param name="width">Desired width of map</param>
        /// <param name="height">Desired Height of Map</param>
        /// <param name="numberOfEntityLayers">How many entity layers to include</param>
        /// <param name="distanceMeasurement">How to measure the distance of a single-tile movement</param>
        /// <param name="layersBlockingWalkability">Layers which should factor into move logic</param>
        /// <param name="layersBlockingTransparency">Layers which should factor into transparency</param>
        /// <param name="entityLayersSupportingMultipleItems">How many entity layers support multiple entities per layer</param>
        protected RogueLikeMapBase(IScreenObject backingObject, int width, int height, int numberOfEntityLayers, Distance distanceMeasurement,
            uint layersBlockingWalkability = uint.MaxValue, uint layersBlockingTransparency = uint.MaxValue,
            uint entityLayersSupportingMultipleItems = uint.MaxValue) : base(width, height, numberOfEntityLayers,
            distanceMeasurement, layersBlockingWalkability, layersBlockingTransparency,
            entityLayersSupportingMultipleItems)
        {
            BackingObject = backingObject;

            ObjectAdded += Object_Added;
            ObjectRemoved += Object_Removed;

            _renderers = new List<ScreenSurface>();
            _surfaceEntityRenderers = new Dictionary<ScreenSurface, Renderer>();
            TerrainView = new LambdaTranslationGridView<IGameObject?, ColoredGlyph>(Terrain, GetTerrainAppearance);

            if (AllComponents != null) // TODO: Workaround for GoRogue bug https://github.com/Chris3606/GoRogue/issues/219
            {
                AllComponents.ComponentAdded += On_GoRogueComponentAdded;
                AllComponents.ComponentRemoved += On_GoRogueComponentRemoved;
            }
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
            var cellSurface = new SettableCellSurface(this, viewWidth, viewHeight);

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
                    Terrain_AppearanceIsDirtySet(terrain, EventArgs.Empty);
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
                    Terrain_AppearanceIsDirtySet(terrain, EventArgs.Empty);
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
            foreach (var surface in _renderers)
                surface.IsDirty = true;
        }

        private static ColoredGlyph GetTerrainAppearance(IGameObject? gameObject)
            => ((RogueLikeCell?) gameObject)?.Appearance ?? _transparentAppearance;
    }
}

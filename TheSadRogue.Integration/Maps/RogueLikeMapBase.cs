using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GoRogue.Components;
using GoRogue.GameFramework;
using GoRogue.SpatialMaps;
using SadConsole;
using SadConsole.Components;
using SadConsole.Entities;
using SadConsole.Input;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace TheSadRogue.Integration.Maps
{
    /// <summary>
    /// Abstract base class for a Map that contains the necessary function to render to one or more ScreenSurfaces.
    /// </summary>
    public abstract class RogueLikeMapBase : Map, IScreenObject
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
        /// The IScreenObject acting as the object for the IScreenObject forwarder implementation.
        /// </summary>
        protected IScreenObject BackingObject { get; set; }

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
        /// Creates a renderer that renders this Map.  When no longer used, <see cref="DisposeOfRenderer"/> must
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
        protected void DisposeOfRenderer(ScreenSurface renderer)
        {
            _renderers.Remove(renderer);
            _surfaceEntityRenderers.Remove(renderer);
        }

        private void Object_Added(object? sender, ItemEventArgs<IGameObject> e)
        {
            switch (e.Item)
            {
                case RogueLikeCell terrain:
                    // Ensure we flag the surfaces of renderers as dirty on the add and on subsequent appearance changed events
                    terrain.AppearanceChanged += Terrain_AppearanceChanged;
                    Terrain_AppearanceChanged(terrain, EventArgs.Empty);
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
                    terrain.AppearanceChanged -= Terrain_AppearanceChanged;
                    Terrain_AppearanceChanged(terrain, EventArgs.Empty);
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

        private void Terrain_AppearanceChanged(object? sender, EventArgs e)
        {
            foreach (var surface in _renderers)
                surface.IsDirty = true;
        }

        private static ColoredGlyph GetTerrainAppearance(IGameObject? gameObject)
            => ((RogueLikeCell?) gameObject)?.Appearance ?? _transparentAppearance;

        #region IScreenObject Implementation
        /// <inheritdoc/>
        public void Render(TimeSpan delta) => BackingObject.Render(delta);

        /// <inheritdoc/>
        public void OnFocused() => BackingObject.OnFocused();

        /// <inheritdoc/>
        public void OnFocusLost() => BackingObject.OnFocusLost();

        /// <inheritdoc/>
        TComponent IScreenObject.GetSadComponent<TComponent>() => BackingObject.GetSadComponent<TComponent>();

        /// <inheritdoc/>
        IEnumerable<TComponent> IScreenObject.GetSadComponents<TComponent>() => BackingObject.GetSadComponents<TComponent>();

        /// <inheritdoc/>
        bool IScreenObject.HasSadComponent<TComponent>(out TComponent component) => BackingObject.HasSadComponent(out component);

        public bool ProcessKeyboard(Keyboard keyboard) => BackingObject.ProcessKeyboard(keyboard);

        /// <inheritdoc/>
        public bool ProcessMouse(MouseScreenObjectState state) => BackingObject.ProcessMouse(state);

        /// <inheritdoc/>
        public void LostMouse(MouseScreenObjectState state) => BackingObject.LostMouse(state);

        /// <summary>
        /// Calls Update for all entities, then Updates all SadComponents and Children. Only processes if IsEnabled is
        /// true.
        /// </summary>
        /// <param name="delta">Time since last update.</param>
        public void Update(TimeSpan delta)
        {
            if (!IsEnabled) return;

            foreach (var entity in Entities.Items)
            {
                // Guaranteed to succeed since all must be RoguelikeEntities
                var scEntity = (Entity)entity;
                scEntity.Update(delta);
            }

            BackingObject.Update(delta);
        }

        /// <inheritdoc/>
        public void UpdateAbsolutePosition() => BackingObject.UpdateAbsolutePosition();

        /// <inheritdoc/>
        public FocusBehavior FocusedMode
        {
            get => BackingObject.FocusedMode;
            set => BackingObject.FocusedMode = value;
        }

        /// <inheritdoc/>
        public Point AbsolutePosition => BackingObject.AbsolutePosition;

        /// <inheritdoc/>
        public ScreenObjectCollection Children => BackingObject.Children;

        /// <inheritdoc/>
        public ObservableCollection<IComponent> SadComponents => BackingObject.SadComponents;

        /// <inheritdoc/>
        public bool IsEnabled
        {
            get => BackingObject.IsEnabled;
            set => BackingObject.IsEnabled = value;
        }

        /// <inheritdoc/>
        public bool IsExclusiveMouse
        {
            get => BackingObject.IsExclusiveMouse;
            set => BackingObject.IsExclusiveMouse = value;
        }

        /// <inheritdoc/>
        public bool IsFocused
        {
            get => BackingObject.IsFocused;
            set => BackingObject.IsFocused = value;
        }

        /// <inheritdoc/>
        public bool IsVisible
        {
            get => BackingObject.IsVisible;
            set => BackingObject.IsVisible = value;
        }

        /// <inheritdoc/>
        public IScreenObject Parent
        {
            get => BackingObject.Parent;
            set => BackingObject.Parent = value;
        }

        /// <inheritdoc/>
        public Point Position
        {
            get => BackingObject.Position;
            set => BackingObject.Position = value;
        }

        /// <inheritdoc/>
        public bool UseKeyboard
        {
            get => BackingObject.UseKeyboard;
            set => BackingObject.UseKeyboard = value;
        }

        /// <inheritdoc/>
        public bool UseMouse
        {
            get => BackingObject.UseMouse;
            set => BackingObject.UseMouse = value;
        }

        /// <inheritdoc/>
        public event EventHandler EnabledChanged
        {
            add => BackingObject.EnabledChanged += value;
            remove => BackingObject.EnabledChanged -= value;
        }

        /// <inheritdoc/>
        public event EventHandler<ValueChangedEventArgs<IScreenObject>> ParentChanged
        {
            add => BackingObject.ParentChanged += value;
            remove => BackingObject.ParentChanged -= value;
        }

        /// <inheritdoc/>
        public event EventHandler<ValueChangedEventArgs<Point>> PositionChanged
        {
            add => BackingObject.PositionChanged += value;
            remove => BackingObject.PositionChanged -= value;
        }

        /// <inheritdoc/>
        public event EventHandler VisibleChanged
        {
            add => BackingObject.VisibleChanged += value;
            remove => BackingObject.VisibleChanged -= value;
        }

        /// <inheritdoc/>
        public event EventHandler FocusLost
        {
            add => BackingObject.FocusLost += value;
            remove => BackingObject.FocusLost -= value;
        }

        /// <inheritdoc/>
        public event EventHandler Focused
        {
            add => BackingObject.Focused += value;
            remove => BackingObject.Focused -= value;
        }
        #endregion
    }
}

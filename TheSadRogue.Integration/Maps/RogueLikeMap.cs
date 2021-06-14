using GoRogue;
using GoRogue.Components;
using GoRogue.GameFramework;
using GoRogue.Pathing;
using SadConsole;
using SadRogue.Primitives;

namespace SadRogue.Integration.Maps
{
    /// <summary>
    /// A GoRogue Map that contains map data and implements necessary functionality to be added to the SadConsole
    /// object hierarchy as a screen object that renders itself.  If you need to render the map independently in
    /// more than one location, use <see cref="AdvancedRogueLikeMap"/> instead.
    /// </summary>
    public class RogueLikeMap : RogueLikeMapBase
    {
        #region Viewport Forwarding Functions
        /// <summary>
        /// The visible portion of the map.
        /// </summary>
        public Rectangle View
        {
            get => ((ScreenSurface)BackingObject).Surface.View;
            set => ((ScreenSurface)BackingObject).Surface.View = value;
        }

        /// <summary>
        /// Gets or sets the width of the visible portion of the map, in tiles.
        /// </summary>
        public int ViewWidth
        {
            get => ((ScreenSurface)BackingObject).Surface.ViewWidth;
            set => ((ScreenSurface)BackingObject).Surface.ViewWidth = value;
        }

        /// <summary>
        /// Gets or sets the height of the visible portion of the map, in tiles.
        /// </summary>
        public int ViewHeight
        {
            get => ((ScreenSurface)BackingObject).Surface.ViewHeight;
            set => ((ScreenSurface)BackingObject).Surface.ViewHeight = value;
        }

        /// <summary>
        /// The position of the currently viewed portion of the map.
        /// </summary>
        public Point ViewPosition
        {
            get => ((ScreenSurface)BackingObject).Surface.ViewPosition;
            set => ((ScreenSurface)BackingObject).Surface.ViewPosition = value;
        }
        #endregion

        /// <summary>
        /// Creates a new RogueLikeMapBase.
        /// </summary>
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
        /// <param name="viewSize">Size of map's viewport.</param>
        /// <param name="font">Font to use to render the map.</param>
        /// <param name="fontSize">Size of font to use to render the map.</param>
        public RogueLikeMap(int width, int height, int numberOfEntityLayers, Distance distanceMeasurement,
                            uint layersBlockingWalkability = uint.MaxValue,
                            uint layersBlockingTransparency = uint.MaxValue,
                            uint entityLayersSupportingMultipleItems = uint.MaxValue,
                            FOV? customPlayerFOV = null, AStar? customPather = null,
                            IComponentCollection? customComponentContainer = null,
                            Point? viewSize = null,
                            IFont? font = null,
                            Point? fontSize = null)
            // Nullability override is safe because value is not used in base constructor and we set it below
            : base(null!, width, height, numberOfEntityLayers, distanceMeasurement, layersBlockingWalkability,
                layersBlockingTransparency, entityLayersSupportingMultipleItems, customPlayerFOV, customPather, customComponentContainer)
        {
            // It is safe to never call DestroyRenderer, because the only reference to the object is in this class, so
            // they must be deallocated at the same time anyway.
            BackingObject = CreateRenderer(viewSize, font, fontSize);
        }
    }
}

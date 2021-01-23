using SadConsole;
using SadRogue.Primitives;

namespace TheSadRogue.Integration.Maps
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
        /// Creates a new RogueLikeMap.
        /// </summary>
        /// <param name="width">Desired width of map</param>
        /// <param name="height">Desired Height of Map</param>
        /// <param name="numberOfEntityLayers">How many entity layers to include</param>
        /// <param name="distanceMeasurement">How to measure the distance of a single-tile movement</param>
        /// <param name="layersBlockingWalkability">Layers which should factor into move logic</param>
        /// <param name="layersBlockingTransparency">Layers which should factor into transparency</param>
        /// <param name="entityLayersSupportingMultipleItems">How many entity layers support multiple entities per layer</param>
        /// <param name="viewSize">Size of map's viewport.</param>
        /// <param name="font">Font to use to render the map.</param>
        /// <param name="fontSize">Size of font to use to render the map.</param>
        public RogueLikeMap(int width, int height, int numberOfEntityLayers, Distance distanceMeasurement,
                            uint layersBlockingWalkability = uint.MaxValue,
                            uint layersBlockingTransparency = uint.MaxValue,
                            uint entityLayersSupportingMultipleItems = uint.MaxValue,
                            Point? viewSize = null,
                            Font? font = null,
                            Point? fontSize = null)
            // Nullability override is safe because value is not used in base constructor and we set it below
            : base(null!, width, height, numberOfEntityLayers, distanceMeasurement, layersBlockingWalkability,
                layersBlockingTransparency, entityLayersSupportingMultipleItems)
        {
            // It is safe to never call dispose, because the only reference to the object is in this class.
            BackingObject = CreateRenderer(viewSize, font, fontSize);
        }
    }
}

using System;
using SadConsole;
using SadRogue.Primitives;

namespace TheSadRogue.Integration.Maps
{
    /// <summary>
    /// A GoRogue Map that contains map data and implements necessary functionality to call Update on its entities
    /// when added to the SadConsole object hierarchy, and to create one or more screen surfaces that render the map
    /// independently.  If you need only one renderer, consider <see cref="RogueLikeMap"/> instead.
    /// </summary>
    /// <remarks>
    /// To properly utilize this class, you must add the map itself (which is a ScreenObject) to the SadConsole object
    /// hierarchy, which ensures that Update is called on entities in the map, AND create one or more renderers that
    /// are added to the SadConsole object hierarchy.
    /// </remarks>
    public class AdvancedRogueLikeMap : RogueLikeMapBase
    {
        /// <summary>
        /// Creates a new AdvancedRogueLikeMap.
        /// </summary>
        /// <param name="width">Desired width of map</param>
        /// <param name="height">Desired Height of Map</param>
        /// <param name="numberOfEntityLayers">How many entity layers to include</param>
        /// <param name="distanceMeasurement">How to measure the distance of a single-tile movement</param>
        /// <param name="layersBlockingWalkability">Layers which should factor into move logic</param>
        /// <param name="layersBlockingTransparency">Layers which should factor into transparency</param>
        /// <param name="entityLayersSupportingMultipleItems">How many entity layers support multiple entities per layer</param>
        public AdvancedRogueLikeMap(int width, int height, int numberOfEntityLayers, Distance distanceMeasurement, uint layersBlockingWalkability = UInt32.MaxValue, uint layersBlockingTransparency = UInt32.MaxValue, uint entityLayersSupportingMultipleItems = UInt32.MaxValue)
            : base(new ScreenObject(), width, height, numberOfEntityLayers, distanceMeasurement, layersBlockingWalkability, layersBlockingTransparency, entityLayersSupportingMultipleItems)
        { }

        public new ScreenSurface CreateRenderer(Point? viewSize = null, Font? font = null, Point? fontSize = null)
            => base.CreateRenderer(viewSize, font, fontSize);

        public new void DisposeOfRenderer(ScreenSurface renderer) => base.DisposeOfRenderer(renderer);
    }
}

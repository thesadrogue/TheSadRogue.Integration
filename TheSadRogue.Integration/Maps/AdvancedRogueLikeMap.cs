using GoRogue.FOV;
using GoRogue.Components;
using GoRogue.GameFramework;
using GoRogue.Pathing;
using SadConsole;
using SadRogue.Primitives;

namespace SadRogue.Integration.Maps
{
    /// <summary>
    /// A GoRogue Map that contains map data and implements necessary functionality to call Update on its entities
    /// when added to the SadConsole object hierarchy, and to create one or more screen surfaces that render the map
    /// independently.  If you need only one renderer, consider <see cref="RogueLikeMap"/> instead.
    /// </summary>
    /// <remarks>
    /// To properly utilize this class, you must add the map itself (which is a ScreenObject) to the SadConsole object
    /// hierarchy, which ensures that Update is called on entities in the map, AND create one or more renderers using
    /// CreateRenderer and add them to the SadConsole object hierarchy.
    /// </remarks>
    public class AdvancedRogueLikeMap : RogueLikeMapBase
    {
        /// <summary>
        /// Creates a new AdvancedRogueLikeMap.
        /// </summary>
        /// <param name="width">Width of map.</param>
        /// <param name="height">Height of the map.</param>
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
        /// Custom A* pathfinder for the map.  Typically, you won't need to specify this; By default, uses
        /// <see cref="Map.WalkabilityView"/> to determine which locations can be reached, and calculates distance based
        /// on the <see cref="Distance" /> passed in via the constructor.
        /// </param>
        /// <param name="customComponentContainer">
        /// A custom component container to use for <see cref="Map.GoRogueComponents"/>.  If not specified, a
        /// <see cref="ComponentCollection"/> is used.  Typically you will not need to specify this, as a
        /// ComponentCollection is sufficient for nearly all use cases.
        /// </param>
        public AdvancedRogueLikeMap(int width, int height, int numberOfEntityLayers, Distance distanceMeasurement,
            uint layersBlockingWalkability = uint.MaxValue, uint layersBlockingTransparency = uint.MaxValue,
            uint entityLayersSupportingMultipleItems = uint.MaxValue, IFOV? customPlayerFOV = null,
            AStar? customPather = null, IComponentCollection? customComponentContainer = null)
            : base(new ScreenObject(), width, height, numberOfEntityLayers, distanceMeasurement, layersBlockingWalkability, layersBlockingTransparency, entityLayersSupportingMultipleItems, customPlayerFOV, customPather, customComponentContainer)
        { }

        /// <inheritdoc cref="RogueLikeMapBase.CreateRenderer"/>
        public IScreenSurface CreateRenderer(Point? viewSize = null, IFont? font = null, Point? fontSize = null)
            => base.CreateRenderer(viewSize, font, fontSize);

        /// <inheritdoc cref="RogueLikeMapBase.DestroyRenderer"/>
        public new void DestroyRenderer(IScreenSurface renderer) => base.DestroyRenderer(renderer);
    }
}

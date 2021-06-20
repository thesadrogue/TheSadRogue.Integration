using GoRogue;
using GoRogue.Components;
using GoRogue.Pathing;
using SadConsole;
using SadConsole.Input;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;

namespace ExampleGame
{
    public class TestMap : RogueLikeMap
    {
        public TestMap(int width, int height, int numberOfEntityLayers, Distance distanceMeasurement,
            uint layersBlockingWalkability = uint.MaxValue,
            uint layersBlockingTransparency = uint.MaxValue,
            uint entityLayersSupportingMultipleItems = uint.MaxValue,
            FOV? customPlayerFOV = null, AStar? customPather = null,
            IComponentCollection? customComponentContainer = null,
            Point? viewSize = null,
            IFont? font = null,
            Point? fontSize = null)
            : base(width, height, numberOfEntityLayers, distanceMeasurement, layersBlockingWalkability,
                layersBlockingTransparency, entityLayersSupportingMultipleItems, customPlayerFOV, customPather,
                customComponentContainer, viewSize, font, fontSize)
        {
            UseMouse = true;
        }

        protected override bool ProcessMouse(MouseScreenObjectState state)
        {
            System.Diagnostics.Debug.WriteLine("ProcessMouse called!");

            return base.ProcessMouse(state);
        }
    }
}
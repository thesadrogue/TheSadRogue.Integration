using System;
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
        private int _count = 0;
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
            //UseMouse = true;
        }

        public override void Update(TimeSpan delta)
        {
            System.Diagnostics.Debug.WriteLine($"TestMap Update {_count++}!");

            base.Update(delta);
        }
    }
}

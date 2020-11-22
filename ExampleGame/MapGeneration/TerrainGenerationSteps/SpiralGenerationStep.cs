using System;
using System.Collections.Generic;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SadRogue.Primitives;

namespace ExampleGame.MapGeneration.TerrainGenerationSteps
{
    public class SpiralGenerationStep : GenerationStep
    {
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            Random random = new Random();
            var map = context.GetFirstOrNew<ISettableMapView<bool>>(()=> new ArrayMap<bool>(context.Width, context.Height));
            int originX = random.Next(0, map.Width);
            int originY = random.Next(0, map.Height);
            Point origin = (originX, originY);

            Func<double, Point> spiral = theta => origin + new PolarCoordinate(theta / 3, theta).ToCartesian();
  
            for (double i = 0; i < 100; i += 0.01)
            {
                Point here = spiral(i);
                if (map.Contains(here))
                {
                    map[spiral(i)] = true;
                }
            }

            yield return null;
        }
    }
}
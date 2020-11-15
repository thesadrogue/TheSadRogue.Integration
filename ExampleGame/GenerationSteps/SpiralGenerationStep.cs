using System;
using System.Collections.Generic;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SadRogue.Primitives;

namespace ExampleGame
{
    public class SpiralGenerationStep : GenerationStep
    {
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            Random random = new Random();
            var map = context.GetFirst<ISettableMapView<bool>>();
            int originX = random.Next(0, map.Width);
            int originY = random.Next(0, map.Height);
            Point origin = (originX, originY);

            Func<double, Point> spiral = (theta) => new PolarCoordinate(theta / 4, theta).ToCartesian();
  
            for (double i = 0; i < 100; i += 0.1)
            {
                map[spiral(i)] = true;
            }

            yield return null;
        }
    }
}
using System;
using System.Collections.Generic;
using GoRogue.MapGeneration;
using GoRogue.MapViews;

namespace ExampleGame
{
    public class CaveSeedingStep : GenerationStep
    {
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            Random random = new Random();
            var map = context.GetFirst<ISettableMapView<bool>>();
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    if (random.Next(0, 101) <= 33)
                        map[i, j] = true;
                    else
                        map[i, j] = false;
                }
            }

            yield return null;
        }
    }
}
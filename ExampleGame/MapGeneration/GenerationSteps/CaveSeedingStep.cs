using System;
using System.Collections.Generic;
using GoRogue.MapGeneration;
// using GoRogue.MapViews;
using SadRogue.Primitives.GridViews;

namespace ExampleGame.MapGeneration.GenerationSteps
{
    public class CaveSeedingStep : GenerationStep
    {
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            Random random = new Random();
            var map = context.GetFirstOrNew<ISettableGridView<bool>>(()=> new ArrayView<bool>(context.Width, context.Height));
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    if (random.Next(0, 101) <= 50)
                        map[i, j] = true;
                    else
                        map[i, j] = false;
                }
            }

            yield return null;
        }
    }
}
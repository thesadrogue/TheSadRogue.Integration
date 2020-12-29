using System;
using System.Collections.Generic;
using GoRogue.MapGeneration;

namespace ExampleGame.MapGeneration.GenerationSteps
{
    public class PlaceCreaturesStep : GenerationStep
    {
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            Random r = new Random();
            int chance = r.Next(0, 101);
            yield return null;
        }
    }
}
using System;
using System.Collections.Generic;
using GoRogue.MapGeneration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace TheSadRogue.Integration.MapGenerationSteps
{
    /// <summary>
    /// Carves a walkable spiral tunnel through unwalkable space
    /// </summary>
    public class SpiralGenerationStep : GenerationStep
    {
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            Random random = new Random();
            var map = context.GetFirstOrNew<ISettableGridView<bool>>(()=> new ArrayView<bool>(context.Width, context.Height));
            int originX = random.Next(0, map.Width);
            int originY = random.Next(0, map.Height);
            Point origin = (originX, originY);

            

            for (double i = 0; i < 100; i += 0.01)
            {
                Point here = Spiral((originX, originY), i);
                if (map.Contains(here))
                {
                    map[Spiral(origin, i)] = true;
                }
            }

            yield return null;
        }
        
        /// <summary>
        /// How to calculate the current position of the spiral
        /// </summary>
        /// <param name="origin">the starting point for the spiral</param>
        /// <param name="theta">the current degree of rotation (in radians) around the origin.</param>
        /// <returns>the cartesian point along the spiral given the current theta</returns>
        private Point Spiral(Point origin, double theta) => 
            origin + new PolarCoordinate(theta / 3, theta).ToCartesian();
    }
}
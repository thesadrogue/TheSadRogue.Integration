using System.Collections.Generic;
using GoRogue.MapGeneration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace TheSadRogue.Integration.MapGenerationSteps
{
    /// <summary>
    /// Carves a walkable spiral tunnel through non-walkable space
    /// </summary>
    public class SpiralGenerationStep : GenerationStep
    {
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var map = context.GetFirstOrNew<ISettableGridView<bool>>
                (()=> new ArrayView<bool>(context.Width, context.Height));

            Point origin = (map.Width / 2, map.Height / 2);


            double increment = 0.001;
            for (double i = 0; i < 125; i += increment)
            {
                Point here = Spiral(origin, i);
                if (map.Contains(here))
                {
                    map[here] = true;
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
            origin + new PolarCoordinate(theta / 3, theta);
    }
}
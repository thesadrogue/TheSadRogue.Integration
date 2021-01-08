using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace TheSadRogue.Integration.MapGenerationSteps
{
    /// <summary>
    /// Creates walkable parallelogram-shaped rooms
    /// </summary>
    public class ParallelogramGenerationStep : GenerationStep
    {
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var map = context.GetFirstOrNew<ISettableGridView<bool>>
                (() => new ArrayView<bool>(context.Width, context.Height));

            int width = 10;
            int height = 5;
            for (int i = -5; i < map.Width; i += width)
            {
                for (int j = 0; j < map.Height; j += height)
                {
                    var origin = new Point(i, j);
                    var region = Region.RegularParallelogram("parallelogram", origin, width, height, 0);
                    
                    foreach (var point in region.InnerPoints.Positions.Where(p=> map.Contains(p)))
                        map[point] = true;
                    
                    yield return null;
                }
            }
            
            
            yield return null;
        }
    }
}
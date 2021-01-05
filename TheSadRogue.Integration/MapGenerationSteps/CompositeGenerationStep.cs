using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.Steps;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace TheSadRogue.Integration.MapGenerationSteps
{
    /// <summary>
    /// This generation step sections the map into regions and then applies a different generation step to each region.
    /// </summary>
    public class CompositeGenerationStep : GenerationStep
    {
        private readonly Random _random;
        private readonly IEnumerable<Region> _regions;
        private readonly int _width;
        private readonly int _height;

        private readonly List<GenerationStep[]> _stepSets;
        public CompositeGenerationStep(int width, int height)
        {
            _width = width;
            _height = height;
            _stepSets = new List<GenerationStep[]>()
            {
                DefaultAlgorithms.DungeonMazeMapSteps(null, 0, 0).ToArray(),
                new GenerationStep[] { new BackroomGenerationStep() },
                new GenerationStep[] { new ParallelogramGenerationStep() },
                new GenerationStep[] { new SpiralGenerationStep() },

                new GenerationStep[]
                {
                    new RandomViewFill(),
                    new CellularAutomataAreaGeneration(),
                    new CellularAutomataAreaGeneration(),
                },
            };
            _random = new Random();
            _regions = GenerateRegions();
        }
        
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var map = context.GetFirstOrNew<ISettableGridView<bool>>
                (() => new ArrayView<bool>(context.Width, context.Height));

            foreach (var region in _regions)
            {
                var generator = new Generator(region.Width, region.Height);
                generator.AddSteps(SelectSteps());
                generator = generator.Generate();
                var subMap = generator.Context.GetFirst<ISettableGridView<bool>>();
                
                foreach (var here in region.Points)
                {
                    if (map.Contains(here))
                    {
                        map[here] = subMap[here - (region.Left, region.Top)];
                    }
                }

                yield return null;
            }
            yield return null;
        }

        private IEnumerable<GenerationStep> SelectSteps() => _stepSets[_random.Next(0, _stepSets.Count)];

        /// <summary>
        /// Sections the map into regions of equal size
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Region> GenerateRegions()
        {
            double rotationAngle = 45;//_random.Next(360);
            int minimumDimension = 14;//_random.Next(25, 50);

            var wholeMap = new Rectangle(-_width, -_height,_width * 2,_height * 2);
            foreach (var room in wholeMap.BisectRecursive(minimumDimension))
                yield return Region.FromRectangle("room", room).Rotate(rotationAngle);
        }
    }
}
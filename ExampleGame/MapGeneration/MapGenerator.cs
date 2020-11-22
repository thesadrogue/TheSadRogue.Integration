using System;
using System.Collections.Generic;
using System.Linq;
using ExampleGame.MapGeneration.GenerationSteps;
using ExampleGame.MapGeneration.TerrainGenerationSteps;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SadRogue.Primitives;
using TheSadRogue.Integration;
using RogueLikeEntity = TheSadRogue.Integration.RogueLikeEntity;

namespace ExampleGame.MapGeneration
{
	public class MapGenerator
	{
		private Random _random;
		public int MapWidth { get; }
		public int MapHeight { get; }
		public int PercentAreWalls { get; }
		private Generator _generator;
		
		public MapGenerator(int mapWidth, int mapHeight, int percentWalls = 40)
		{
			MapWidth = mapWidth;
			MapHeight = mapHeight;
			PercentAreWalls = percentWalls;
			_generator = new Generator(mapWidth, mapHeight);
			_random = new Random();
		}
        public RogueLikeMap GenerateMap()
        {
            RogueLikeMap map = new RogueLikeMap(MapWidth,MapHeight, 4, Distance.Manhattan);

            IEnumerable<Region> regions = GenerateRegions();

            foreach (var region in regions)
            {
	            if (region.Top >= 0 || region.Left >= 0 || region.Bottom < MapHeight || region.Right < MapWidth)
	            {
		            int chance = _random.Next(0, 101);
		            int floorGlyph;
		            int wallGlyph;
		            IEnumerable<GenerationStep> steps;
		             if (chance < 20)
		             {
			            steps = GetCaveSteps();
			            floorGlyph = ',';
			            wallGlyph = '&';
		             }
		             else if (chance < 40)
		             {
			             steps = GetCryptSteps();
			             floorGlyph = '.';
			             wallGlyph = '#';
		             }
		            else 
		             if (chance < 60)
		             {
			             steps = GetBackroomsSteps();
			             floorGlyph = '+';
			             wallGlyph = '&';
		             }
		             else if (chance < 80)
		             {
			            steps = GetHallSteps();
			            floorGlyph = '`';
			            wallGlyph = '#';
		             }
		             else
		             {
			            steps = GetSpiralSteps();
			            floorGlyph = ',';
			            wallGlyph = '&';
		            }
		            
		            _generator = new Generator(region.Width, region.Height);
		            _generator.AddSteps(steps);
		            _generator = _generator.Generate();
		            
		            var minimap = _generator.Context.GetFirst<ISettableMapView<bool>>();
		            for (int i = 0; i < minimap.Width; i++)
		            {
			            for (int j = 0; j < minimap.Height; j++)
			            {
				            Point here = (i + region.Left, j + region.Top);
				            bool walkable = minimap[i, j];
				            if (region.Contains(here) && map.Contains(here))
				            {
					            map.SetTerrain(new RogueLikeEntity(here, walkable ? floorGlyph : wallGlyph, walkable, walkable));
				            }
			            }
		            }

	            }
            }

            foreach (var here in map.Terrain.Positions().Where((nil) => !map[nil].Any()))
            {
	            map.SetTerrain(new RogueLikeEntity(here, '`'));
            }
            return map;
        }

        private IEnumerable<Region> GenerateRegions()
        {
	        double rotationAngle = _random.Next(360);
	        int minimumDimension = _random.Next(4, 16);

	        Rectangle wholeMap = new Rectangle(-MapWidth, -MapHeight,MapWidth * 2,MapHeight * 2);
	        Point center = (MapWidth / 2, MapHeight / 2);
            
	        foreach (var room in wholeMap.BisectRecursive(minimumDimension))
		        yield return Region.FromRectangle("room", room).Rotate(rotationAngle, center);
	        
        }

        private IEnumerable<GenerationStep> GetCaveSteps()
        {
	        yield return new CaveSeedingStep();
	        yield return new CaveGenerationStep();
	        yield return new CaveGenerationStep();
	        yield return new CaveGenerationStep();
        }

        private IEnumerable<GenerationStep> GetHallSteps() 
	        => DefaultAlgorithms.DungeonMazeMapSteps(null, 0, 0);

        private IEnumerable<GenerationStep> GetBackroomsSteps()
        {
	        yield return new BackroomGenerationStep();
        }
        private IEnumerable<GenerationStep> GetCryptSteps()
        {
	        yield return new CryptGenerationStep();
        }
        private IEnumerable<GenerationStep> GetSpiralSteps()
        {
	        yield return new SpiralGenerationStep();
        }
	}
}
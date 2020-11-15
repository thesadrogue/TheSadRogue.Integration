using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SadRogue.Primitives;
using TheSadRogue.Integration;

namespace ExampleGame
{
	public class MapGenerator
	{
		private Random _random;
		public int MapWidth { get; }
		public int MapHeight { get; }
		public int PercentAreWalls { get; }
		private Generator _generator;
		public ArrayMap<bool> Map;
		
		private ArrayMap<bool> _caveMap;
		private ArrayMap<bool> _mazeMap;
		private ArrayMap<bool> _backroomsMap;
		private ArrayMap<bool> _cryptMap;
		private ArrayMap<bool> _spiralMap;
		
		public MapGenerator(int mapWidth, int mapHeight, int percentWalls = 40)
		{
			MapWidth = mapWidth;
			MapHeight = mapHeight;
			PercentAreWalls = percentWalls;
			
			Map = new ArrayMap<bool>(mapWidth, mapHeight);
			_caveMap = new ArrayMap<bool>(mapWidth, mapHeight);
			_mazeMap = new ArrayMap<bool>(mapWidth, mapHeight);
			_backroomsMap = new ArrayMap<bool>(mapWidth, mapHeight);
			_cryptMap = new ArrayMap<bool>(mapWidth, mapHeight);
			_spiralMap = new ArrayMap<bool>(mapWidth, mapHeight);

			_random = new Random();
		}
        public RogueLikeMap GenerateMap()
        {
	        /*
	         * Intended implementation (not yet done):
	         * - Create a great, big grid of regions that encompasses the entire map.
	         * - For each one of those regions that is within the map,
	         *   - Pick a random generation algorithm
	         *   - Perform that algorithm's generation step on this region
	         */
            RogueLikeMap map = new RogueLikeMap(MapWidth,MapHeight, 4, Distance.Manhattan);

            IEnumerable<Region> regions = GenerateRegions();
            List<Region> mazeSections = new List<Region>();
            foreach (var region in regions)
            {
                int chance = _random.Next(0, 100);
                if (chance < 40)
                    DrawRoom(map, region);
                else if (chance < 66)
                    DrawCave(map, region);
                else
                    mazeSections.Add(region);
            }

            foreach (var region in mazeSections)
            {
                DrawMaze(map, region);
            }
            return map;
        }

        private IEnumerable<GenerationStep> GetCaveSteps()
        {
	        yield return new CaveSeedingStep();
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

		#region cavern
		private void DrawCave(RogueLikeMap map, Region region)
		{
			foreach (var point in region.Points)
			{
				if (map.Contains(point))
				{
					if (Map[point.X, point.Y])
					{
						map.SetTerrain(new RogueLikeEntity(point, '%', false, false));
					}
					else
					{
						map.SetTerrain(new RogueLikeEntity(point, ','));
					}
				}
			}
		}
		#endregion
		
		#region maze
        private void DrawMaze(RogueLikeMap map, Region region)
        {
	        var _mazeGenerator = new Generator(region.Width, region.Height);
	        _mazeGenerator.AddSteps(DefaultAlgorithms.DungeonMazeMapSteps(minRooms: 0, maxRooms: 0, maxCreationAttempts: 1, saveDeadEndChance: 60));

	        foreach(var step in _mazeGenerator.GenerationSteps)
	        {
		        step.PerformStep(_mazeGenerator.Context);
	        }
	        
            var mazeView = _mazeGenerator.Context.GetFirst<IMapView<bool>>();
            foreach (var position in region.Points)
            {
                if (map.Contains(position) && mazeView.Contains(position))
                {
                    if (mazeView[position])
                    {
                        map.SetTerrain(new RogueLikeEntity(position, '.'));
                    }
                    else
                    {
                        map.SetTerrain(new RogueLikeEntity(position, '#', false, false));
                    }
                }
            }
        }

        #endregion
        
        #region backrooms
        private void DrawRoom(RogueLikeMap map, Region region)
        {
            foreach (var point in region.InnerPoints.Positions)
            {
                if (map.Contains(point))
                {
                    var terrain = new RogueLikeEntity(point, '.', true, true);
                    map.SetTerrain(terrain);
                }
            }

            foreach (var point in region.OuterPoints.Positions)
            {
                if (map.Contains(point))
                {
                    var terrain = new RogueLikeEntity(point, '#', false, false);
                    map.SetTerrain(terrain);
                }
            }

            foreach (var p in map.Positions().Where(p => map.GetTerrainAt(p) == null))
            {
                var terrain = new RogueLikeEntity(p, '.', true, true);
                map.SetTerrain(terrain);
            }
        }

        private IEnumerable<Region> GenerateRegions()
        {
            double rotationAngle = _random.Next(360);
            int minimumDimension = _random.Next(4, 16);
            var scene = new RogueLikeMap(MapWidth,MapHeight, 31, Distance.Manhattan);

            Rectangle wholeMap = new Rectangle(-MapWidth/2, -MapHeight/2,MapWidth * 3 / 2,MapHeight * 3 / 2);
            Point center = (MapWidth / 2, MapHeight / 2);
            
            foreach (var room in wholeMap.BisectRecursive(12))
	            yield return Region.FromRectangle("room", room).Rotate(rotationAngle, center);
	        
        }
        #endregion
	}
}
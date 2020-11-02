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
		Random _random = new Random();
		public ArrayMap<bool> Map;
		private ArrayMap<bool> _caveMap;
		private ArrayMap<bool> _mazeMap;
		public int MapWidth { get; }
		public int MapHeight { get; }
		public int PercentAreWalls { get; }

		public MapGenerator(int mapWidth, int mapHeight, int percentWalls = 40)
		{
			MapWidth = mapWidth;
			MapHeight = mapHeight;
			PercentAreWalls = percentWalls;

			Map = new ArrayMap<bool>(mapWidth, mapHeight);
			_caveMap = new ArrayMap<bool>(mapWidth, mapHeight);
			_mazeMap = new ArrayMap<bool>(mapWidth, mapHeight);
		}
        public RogueLikeMap GenerateMap()
        {
            Random r = new Random();
            RogueLikeMap map = new RogueLikeMap(MapWidth,MapHeight, 4, Distance.Manhattan);
            MakeCavernBackground();
            IEnumerable<Region> regions = GenerateBackrooms();
            List<Region> mazeSections = new List<Region>();
            foreach (var region in regions)
            {
                int chance = r.Next(0, 100);
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


		#region cavern
		private void DrawCave(RogueLikeMap map, Region region)
		{
			foreach (var point in region.Points)
			{
				if (map.Contains(point) && !IsOutOfBounds(point.X, point.Y))
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
		public void MakeCavernBackground()
		{
			FillRandomCells();
			for (int row = 0; row <= MapHeight - 1; row++)
			{
				for (int column = 0; column <= MapWidth - 1; column++)
				{
					_caveMap[column, row] = PlaceWallLogic(column, row);
				}
			}
		}

		private void FillRandomCells()
		{
			for (int row = 0; row <= MapHeight - 1; row++)
			{
				for (int column = 0; column <= MapWidth - 1; column++)
				{
					if (row == 0 || row == MapHeight - 1 || column == 0 || column == MapWidth - 1)
						_caveMap[column, row] = false;
					else
						_caveMap[column, row] = _random.Next(0, 2) % 2 == 0;
				}
			}
		}

		public bool PlaceWallLogic(int x, int y)
		{
			int numWalls = GetAdjacentWalls(x, y, 1, 1);
			if (Map[x, y])
				return numWalls >= 4;

			return numWalls >= 5;
		}

		public int GetAdjacentWalls(int x, int y, int scopeX, int scopeY)
		{
			int startX = x - scopeX;
			int startY = y - scopeY;
			int endX = x + scopeX;
			int endY = y + scopeY;

			int iX = startX;
			int iY = startY;

			int wallCounter = 0;

			for (iY = startY; iY <= endY; iY++)
			{
				for (iX = startX; iX <= endX; iX++)
				{
					if (!(iX == x && iY == y))
					{
						if (IsWall(iX, iY))
						{
							wallCounter += 1;
						}
					}
				}
			}

			return wallCounter;
		}

		public bool IsWall(int x, int y) => IsOutOfBounds(x, y) || Map[x, y];

		public bool IsOutOfBounds(int x, int y) => x < 0 || y < 0 || x > MapWidth - 1 || y > MapHeight - 1;

		bool RandomPercent(int percent) => percent >= _random.Next(1, 101);
		#endregion
		
		#region maze
        private void DrawMaze(RogueLikeMap map, Region region)
        {
	        var _mazeGenerator = new Generator(MapWidth, MapHeight);
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

        private IEnumerable<Region> GenerateBackrooms()
        {
            double rotationAngle = _random.NextDouble() * 30;
            var scene = new RogueLikeMap(MapWidth,MapHeight, 31, Distance.Manhattan);
            int xOffset = _random.Next(-15, 15) - 30;
            int yOffset = _random.Next(-15, 15) - 30;
            
            Rectangle wholeMap = new Rectangle(xOffset, yOffset,MapWidth + xOffset,MapHeight +yOffset);
            Point center = (MapWidth / 2, MapHeight / 2);
            
            foreach (var room in wholeMap.BisectRecursive(12))
	            yield return Region.FromRectangle("room", room).Rotate(rotationAngle, center);
	        
        }
        #endregion
	}
}
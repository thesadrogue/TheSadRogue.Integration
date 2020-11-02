using System;
using System.Collections.Generic;
using GoRogue.MapViews;

namespace ExampleGame
{
	//stole from roguebasin.com
	//don't let this end up in the final version
	public class MapGenerator
	{
		Random rand = new Random();

		public ArrayMap<bool> Map;
		public int MapWidth { get; set; }
		public int MapHeight { get; set; }
		public int PercentAreWalls { get; set; }

		public MapGenerator(int mapWidth, int mapHeight, int percentWalls = 40)
		{
			MapWidth = mapWidth;
			MapHeight = mapHeight;
			PercentAreWalls = percentWalls;
			Map = new ArrayMap<bool>(mapWidth, mapHeight);
		}

		public void MakeCaverns()
		{
			FillRandomCells();
			for (int row = 0; row <= MapHeight - 1; row++)
			{
				for (int column = 0; column <= MapWidth - 1; column++)
				{
					Map[column, row] = PlaceWallLogic(column, row);
				}
			}
		}

		private void FillRandomCells()
		{
			Random r = new Random();
			for (int row = 0; row <= MapHeight - 1; row++)
			{
				for (int column = 0; column <= MapWidth - 1; column++)
				{
					if (row == 0 || row == MapHeight - 1 || column == 0 || column == MapWidth - 1)
						Map[column, row] = false;
					else
						Map[column, row] = r.Next(0, 2) % 2 == 0;
				}
			}
		}

		public bool PlaceWallLogic(int x, int y)
		{
			int numWalls = GetAdjacentWalls(x, y, 1, 1);
			
			if (Map[x, y])
			{
				if (numWalls >= 4)
				{
					return true;
				}

				if (numWalls < 2)
				{
					return false;
				}

			}
			else
			{
				if (numWalls >= 5)
				{
					return true;
				}
			}

			return false;
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

		public bool IsWall(int x, int y)
		{
			// Consider out-of-bound a wall
			if (IsOutOfBounds(x, y))
			{
				return true;
			}

			if (Map[x, y])
			{
				return true;
			}

			if (!Map[x, y])
			{
				return false;
			}

			return false;
		}

		public bool IsOutOfBounds(int x, int y)
		{
			if (x < 0 || y < 0)
			{
				return true;
			}
			else if (x > MapWidth - 1 || y > MapHeight - 1)
			{
				return true;
			}

			return false;
		}

		int RandomPercent(int percent)
		{
			if (percent >= rand.Next(1, 101))
			{
				return 1;
			}

			return 0;
		}
	}
}
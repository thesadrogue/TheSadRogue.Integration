namespace ExampleGame
{
    /// <summary>
    /// A tiny game to give examples of how to use GoRogue
    /// </summary>
    class Program
    {
        public const int Width = 80;
        public const int Height = 25;
        private const int _mapWidth = 200;
        private const int _mapHeight = 125;

        static void Main(string[] args)
        {
            SadConsole.Game.Create(Width, Height);
            SadConsole.Game.Instance.OnStart = Init;
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        /// <summary>
        /// Runs before the game starts
        /// </summary>
        private static void Init()
        {
            var ui = new GameUi(Width, Height, _mapWidth, _mapHeight);
            SadConsole.GameHost.Instance.Screen.Children.Add(ui);
        }
    }
}

/* The Example one-page game */
/*
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SadConsole;
using SadRogue.Primitives;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Components;
using TheSadRogue.Integration.Extensions;

namespace ExampleGame
{
    class Program
    {
        public const int Width = 80;
        public const int Height = 25;
        private const int MapWidth = 80;
        private const int MapHeight = 25;

        static void Main(string[] args)
        {
            Game.Create(Width, Height);
            Game.Instance.OnStart = Init;
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static void Init()
        {
            var map = new RogueLikeMap(MapWidth, MapHeight, 4, Distance.Manhattan);
            GenerateMap(map);
            
            var player = new RogueLikeEntity( (16,16), 1, layer: 1);
            player.AddComponent(new PlayerControlsComponent());
            map.AddEntity(player);

            var screen = new ScreenSurface(MapWidth, MapHeight, map.TerrainSurface.ToArray());
            GameHost.Instance.Screen.Children.Add(screen);
        }
        
        static void GenerateMap(RogueLikeMap map)
        {
            var generator = new Generator(map.Width, map.Height);
            generator.AddSteps(DefaultAlgorithms.DungeonMazeMapSteps());
            generator = generator.Generate();
		      
            var underlyingMap = generator.Context.GetFirst<ISettableMapView<bool>>();
            for (int i = 0; i < underlyingMap.Width; i++)
            {
                for (int j = 0; j < underlyingMap.Height; j++)
                {
                    Point here = (i, j);
                    bool walkable = underlyingMap[i, j];
                    map.SetTerrain(new RogueLikeEntity(here, walkable ? '.' : '#', walkable, walkable));
                }
            }
        }
    }
}
*/
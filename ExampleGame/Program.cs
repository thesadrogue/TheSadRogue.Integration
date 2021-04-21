using System.Linq;
using GoRogue.MapGeneration;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using SadRogue.Integration;
using SadRogue.Integration.FieldOfView;
using SadRogue.Integration.Maps;


namespace ExampleGame
{
    /// <summary>
    /// A tiny game to give examples of how to use GoRogue
    /// </summary>
    class Program
    {
        public const int Width = 80;
        public const int Height = 25;
        private const int MapWidth = 100;
        private const int MapHeight = 60;

        // Initialized in Init, so null-override is used.
        public static RogueLikeMap Map = null!;
        public static Player PlayerCharacter = null!;
        static void Main(/*string[] args*/)
        {
            Game.Create(Width, Height);
            Game.Instance.OnStart = Init;
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        /// <summary>
        /// Runs before the game starts
        /// </summary>
        private static void Init()
        {
            // Generate map
            Map = GenerateMap();

            // Generate player and add to map, recalculating FOV afterwards
            PlayerCharacter = GeneratePlayerCharacter();
            Map.AddEntity(PlayerCharacter);
            PlayerCharacter.CalculateFOV();

            // Center view on player
            Map.AllComponents.Add(new SadConsole.Components.SurfaceComponentFollowTarget { Target = PlayerCharacter });

            GameHost.Instance.Screen = Map;
        }

        private static RogueLikeMap GenerateMap()
        {
            // Generate a rectangular map for the sake of testing.
            var generator = new Generator(MapWidth, MapHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(DefaultAlgorithms.RectangleMapSteps());
                });

            var generatedMap = generator.Context.GetFirst<ISettableGridView<bool>>("WallFloor");

            RogueLikeMap map = new RogueLikeMap(MapWidth, MapHeight, 4, Distance.Manhattan, viewSize: (Width, Height));
            map.AllComponents.Add(new DefaultFieldOfViewHandler());

            foreach(var location in map.Positions())
            {
                bool walkable = generatedMap[location];
                int glyph = walkable ? '.' : '#';
                map.SetTerrain(new RogueLikeCell(location, Color.White, Color.Black, glyph, 0, walkable, walkable));
            }

            return map;
        }

        private static Player GeneratePlayerCharacter()
        {
            var position = Map.WalkabilityView.Positions().First(p => Map.WalkabilityView[p]);
            var player = new Player(position);

            return player;
        }
    }
}

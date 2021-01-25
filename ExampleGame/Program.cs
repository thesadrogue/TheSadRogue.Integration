using System.Linq;
using GoRogue.MapGeneration;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Components;
using TheSadRogue.Integration.Maps;

#pragma warning disable 8618

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
        public static RogueLikeMap Map;
        public static RogueLikeEntity PlayerCharacter;
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

            // Generate player and add to map
            PlayerCharacter = GeneratePlayerCharacter();
            Map.AddEntity(PlayerCharacter);

            // Center view on player
            // TODO: Null override to work around a GoRogue bug.
            Map.AllComponents!.Add(new SadConsole.Components.SurfaceComponentFollowTarget { Target = PlayerCharacter });

            GameHost.Instance.Screen = Map;
        }

        private static RogueLikeMap GenerateMap()
        {
            // Generate a rectangular map for the sake of testing.
            var generator = new Generator(MapWidth, MapHeight)
                .AddSteps(DefaultAlgorithms.RectangleMapSteps())
                .Generate();

            var generatedMap = generator.Context.GetFirst<ISettableGridView<bool>>("WallFloor");

            RogueLikeMap map = new RogueLikeMap(MapWidth, MapHeight, 4, Distance.Euclidean, viewSize: (Width, Height));

            foreach(var location in map.Positions())
            {
                bool walkable = generatedMap[location];
                int glyph = walkable ? '.' : '#';
                map.SetTerrain(new RogueLikeCell(location, Color.White, Color.Black, glyph, 0, walkable, walkable));
            }

            return map;
        }

        private static RogueLikeEntity GeneratePlayerCharacter()
        {
            var position = Map.WalkabilityView.Positions().First(p => Map.WalkabilityView[p]);
            var player = new RogueLikeEntity(position, 1, false);

            var motionControl = new PlayerControlsComponent();
            player.AllComponents.Add(motionControl);
            player.IsFocused = true;
            return player;
        }
    }
}

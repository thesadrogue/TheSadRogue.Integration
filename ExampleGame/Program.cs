using System.Linq;
using GoRogue.MapGeneration;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Components;
using TheSadRogue.Integration.MapGenerationSteps;
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
        private const int MapWidth = 80;
        private const int MapHeight = 25;
        public static RogueLikeMap Map;
        public static RogueLikeEntity PlayerCharacter;
        public static SettableCellSurface MapWindow;
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
            Map = GenerateMap();
            var cells = new ArrayView<ColoredGlyph>(MapWidth, MapHeight);
            cells.ApplyOverlay(Map.TerrainView);

            MapWindow = new SettableCellSurface(Map, Width, Height);
            // MapWindow.SadComponents.Add(Map.EntityRenderer);
            
            PlayerCharacter = GeneratePlayerCharacter();
            Map.AddEntity(PlayerCharacter);
            GameHost.Instance.Screen = Map.CreateRenderer();
        }
        
        private static RogueLikeMap GenerateMap()
        {
            var generator = new Generator(MapWidth, MapHeight)
                .AddStep(new CompositeGenerationStep(MapWidth, MapHeight))
                .Generate();
            
            var generatedMap = generator.Context.GetFirst<ISettableGridView<bool>>();

            RogueLikeMap map = new RogueLikeMap(MapWidth, MapHeight, 4, Distance.Euclidean);
            
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
            var player = new RogueLikeEntity(position, 1, false, true, 1);

            var motionControl = new PlayerControlsComponent();
            player.AddComponent(motionControl);
            player.IsFocused = true;
            return player;
        }
    }
}
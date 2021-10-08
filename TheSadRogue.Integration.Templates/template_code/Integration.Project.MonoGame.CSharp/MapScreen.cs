using GoRogue.MapGeneration;
using SadConsole;
using SadRogue.Integration;
using SadRogue.Integration.FieldOfView.Memory;
using SadRogue.Integration.Maps;
using SadRogue.Primitives.GridViews;

namespace TheSadRogue.Integration.Templates.MonoGame
{
    internal class MapScreen : ScreenObject
    {
        public readonly MyGameMap Map;
        public readonly RogueLikeEntity Player;
        public readonly MessageLogConsole MessageLog;

        const int MessageLogHeight = 5;

        public MapScreen(int mapWidth, int mapHeight)
        {
            // Generate and assign map as a screen so that it renders
            Map = GenerateDungeon(mapWidth, mapHeight);
            Map.Parent = this;

            // Create message log
            MessageLog = new MessageLogConsole(Program.Width, MessageLogHeight);
            MessageLog.Parent = this;
            MessageLog.Position = new(0, Program.Height - MessageLogHeight);

            // Generate player, add to map at a random walkable position, and calculate initial FOV
            Player = MapObjectFactory.Player();
            Player.Position = Map.WalkabilityView.RandomPosition(true);
            Map.AddEntity(Player);
            Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();

            // Center view on player as they move
            Map.DefaultRenderer?.SadComponents.Add(new SadConsole.Components.SurfaceComponentFollowTarget { Target = Player });

            // Generate 10 enemies
            for (int i = 0; i < 10; i++)
            {
                var enemy = MapObjectFactory.Enemy();
                enemy.Position = Map.WalkabilityView.RandomPosition(true);
                Map.AddEntity(enemy);
            }

            // Make sure the map is focused so that it and the entities can receive keyboard input
            Map.IsFocused = true;
        }

        private MyGameMap GenerateDungeon(int mapWidth, int mapHeight)
        {
            // Generate a rectangular map for the sake of testing with GoRogue's map generation system.
            var generator = new Generator(mapWidth, mapHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(DefaultAlgorithms.RectangleMapSteps());
                });

            var generatedMap = generator.Context.GetFirst<ISettableGridView<bool>>("WallFloor");

            // Create a map structure, specifying the appropriate viewport size for the default renderer
            var map = new MyGameMap(mapWidth, mapHeight, new DefaultRendererParams((Program.Width, Program.Height - MessageLogHeight)));

            // Add a component that will implement a character "memory" system, where tiles will be dimmed when they aren't seen by the player,
            // and remain visible exactly as they were when the player last saw them regardless of changes to their actual appearance,
            // until the player sees them again.
            map.AllComponents.Add(new DimmingMemoryFieldOfViewHandler(0.6f));

            // Translate the GoRogue map generation context's information on the map to appropriate RogueLikeCells.  Our terrain
            // is of type MemoryAwareRogueLikeCells because we are using the integration library's "memory-based" fov visibility system.
            map.ApplyTerrainOverlay(generatedMap, (pos, val) => val ? MapObjectFactory.Floor(pos) : MapObjectFactory.Wall(pos));

            return map;
        }
    }
}

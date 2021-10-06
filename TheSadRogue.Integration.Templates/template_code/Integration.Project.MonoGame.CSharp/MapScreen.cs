using GoRogue.MapGeneration;
using SadConsole;
using SadRogue.Integration;
using SadRogue.Integration.FieldOfView.Memory;
using SadRogue.Integration.Keybindings;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
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
            Player = GeneratePlayerCharacter();
            Player.Position = Map.WalkabilityView.RandomPosition((_, walkable) => walkable);
            Map.AddEntity(Player);
            Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();

            // Center view on player as they move
            Map.DefaultRenderer?.SadComponents.Add(new SadConsole.Components.SurfaceComponentFollowTarget { Target = Player });

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
            var map = new MyGameMap(mapWidth, mapHeight, new DefaultRendererParams((Program.Width, Program.Height - MessageLogHeight)), Distance.Manhattan);

            // Add a component that will implement a character "memory" system, where tiles will be dimmed when they aren't seen by the player,
            // and remain visible exactly as they were when the player last saw them regardless of changes to their actual appearance,
            // until the player sees them again.
            map.AllComponents.Add(new DimmingMemoryFieldOfViewHandler(0.6f));

            // Translate the GoRogue map generation context's information on the map to appropriate RogueLikeCells.  We must use MemoryAwareRogueLikeCells
            // because we are using the integration library's "memory-based" fov visibility system.
            foreach(var location in map.Positions())
            {
                bool walkable = generatedMap[location];
                int glyph = walkable ? '.' : '#';
                map.SetTerrain(new MemoryAwareRogueLikeCell(location, Color.White, Color.Black, glyph, 0, walkable, walkable));
            }

            return map;
        }

        // Generates a RogueLikeEntity with the proper components to be our Player.  It may be useful to implement
        // GoRogue's Factory system if you intend to create all of your map objects via composition like this.
        private RogueLikeEntity GeneratePlayerCharacter()
        {
            // Create player object
            var player = new RogueLikeEntity('@', false, layer: (int)MyGameMap.Layer.Monsters);

            // Add component for controlling player movement via keyboard
            var motionControl = new PlayerKeybindingsComponent();
            motionControl.SetMotions(PlayerKeybindingsComponent.ArrowMotions);
            player.AllComponents.Add(motionControl);

            // Add component for updating map's player FOV as they move
            player.AllComponents.Add(new PlayerFOVController());

            return player;
        }
    }
}

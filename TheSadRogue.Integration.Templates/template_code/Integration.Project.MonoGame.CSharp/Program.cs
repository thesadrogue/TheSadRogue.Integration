﻿using System.Linq;
using GoRogue.MapGeneration;
using SadConsole;
using SadRogue.Integration.FieldOfView.Memory;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace TheSadRogue.Integration.Templates.MonoGame
{
    internal static class Program
    {
        // Window width/height
        public const int Width = 80;
        public const int Height = 25;

        // Map width/height
        private const int MapWidth = 100;
        private const int MapHeight = 60;

        public static RogueLikeMap Map;
        public static Player PlayerCharacter;

        private static void Main()
        {
            Game.Create(Width, Height);
            Game.Instance.OnStart = Init;
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static void Init()
        {
            // Generate map
            Map = GenerateMap();

            // Generate player and add to map, recalculating FOV afterwards
            PlayerCharacter = GeneratePlayerCharacter();
            Map.AddEntity(PlayerCharacter);
            PlayerCharacter.CalculateFOV();

            // Center view on player
            Map.DefaultRenderer?.SadComponents.Add(new SadConsole.Components.SurfaceComponentFollowTarget { Target = PlayerCharacter });

            // Set the map as the active screen so that it processes input and renders itself.
            GameHost.Instance.Screen = Map;
            Map.IsFocused = true;

            // Destroy the default starting console that SadConsole created automatically because we're not using it.
            GameHost.Instance.DestroyDefaultStartingConsole();
        }

        private static RogueLikeMap GenerateMap()
        {
            // Generate a rectangular map for the sake of testing with GoRogue's map generation system.
            var generator = new Generator(MapWidth, MapHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(DefaultAlgorithms.RectangleMapSteps());
                });

            var generatedMap = generator.Context.GetFirst<ISettableGridView<bool>>("WallFloor");

            // Create a RogueLikeMap structure, specifying the appropriate viewport size for the default renderer
            RogueLikeMap map = new RogueLikeMap(MapWidth, MapHeight, new DefaultRendererParams((Width, Height)), 4, Distance.Manhattan);

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

        private static Player GeneratePlayerCharacter()
        {
            // Create a player and position them at the first square encountered that is walkable,
            // for the sake of testing.
            var position = Map.WalkabilityView.Positions().First(p => Map.WalkabilityView[p]);
            var player = new Player(position);

            return player;
        }
    }
}
using GoRogue.Random;
using SadConsole;
using SadRogue.Integration;
using ShaiRandom.Generators;

namespace TheSadRogue.Integration.Templates.MonoGame
{
    internal class MapScreen : ScreenObject
    {
        public readonly MyGameMap Map;
        public readonly RogueLikeEntity Player;
        public readonly MessageLogConsole MessageLog;

        const int MessageLogHeight = 5;

        public MapScreen(MyGameMap map)
        {
            // Record the map we're rendering
            Map = map;

            // Create a renderer for the map, specifying viewport size.  The value in DefaultRenderer is automatically
            // managed by the map, and renders whenever the map is the active screen.
            //
            // CUSTOMIZATION: Pass in custom fonts/viewport sizes here.
            //
            // CUSTOMIZATION: If you want multiple renderers to render the same map, you can call CreateRenderer and
            // manage them yourself; but you must call the map's RemoveRenderer when you're done with these renderers,
            // and you must add any non-default renderers to the SadConsole screen object hierarchy, IN ADDITION
            // to the map itself.
            Map.DefaultRenderer = Map.CreateRenderer((Program.Width, Program.Height - MessageLogHeight));

            // Make the Map (which is also a screen object) a child of this screen.  You MUST have the map as a child
            // of the active screen, even if you are using entirely custom renderers.
            Map.Parent = this;

            // Make sure the map is focused so that it and the entities can receive keyboard input
            Map.IsFocused = true;

            // Generate player, add to map at a random walkable position, and calculate initial FOV
            Player = MapObjectFactory.Player();
            Player.Position = GlobalRandom.DefaultRNG.RandomPosition(Map.WalkabilityView, true);
            Map.AddEntity(Player);
            Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();

            // Center view on player as they move
            Map.DefaultRenderer?.SadComponents.Add(new SadConsole.Components.SurfaceComponentFollowTarget { Target = Player });

            // Create message log
            MessageLog = new MessageLogConsole(Program.Width, MessageLogHeight);
            MessageLog.Parent = this;
            MessageLog.Position = new(0, Program.Height - MessageLogHeight);
        }
    }
}

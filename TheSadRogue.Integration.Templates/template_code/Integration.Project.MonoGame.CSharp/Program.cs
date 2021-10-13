using SadConsole;

namespace TheSadRogue.Integration.Templates.MonoGame
{
    /// <summary>
    /// ***********README***********
    ///
    /// The provided code is a simple template to demonstrate some integration library features and set up some boilerplate
    /// for you.  Feel free to use, or delete, any of it that you want; it shows one way of doing things, not the only way!
    ///
    /// The code contains a few comments beginning with "CUSTOMIZATION:", which show you some common points to modify in
    /// order to accomplish some common tasks.  The tags by no means represent a comprehensive guide to everything you
    /// might want to modify; they're simply designed to provide a "quick-start" guide that can help you accomplish some
    /// common tasks.
    /// </summary>
    internal static class Program
    {
        // Window width/height
        public const int Width = 80;
        public const int Height = 25;

        // Map width/height
        private const int MapWidth = 100;
        private const int MapHeight = 60;

        public static MapScreen GameScreen;

        private static void Main()
        {
            Game.Create(Width, Height);
            Game.Instance.OnStart = Init;
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static void Init()
        {
            // Generate a dungeon map
            var map = MapFactory.GenerateDungeonMap(MapWidth, MapHeight);

            // Create a MapScreen and set it as the active screen so that it processes input and renders itself.
            GameScreen = new MapScreen(map);
            GameHost.Instance.Screen = GameScreen;

            // Destroy the default starting console that SadConsole created automatically because we're not using it.
            GameHost.Instance.DestroyDefaultStartingConsole();
        }
    }
}

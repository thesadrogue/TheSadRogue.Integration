using SadConsole;

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
            // Create a Map/MapScreen and as the active screen so that it processes input and renders itself.
            GameScreen = new MapScreen(MapWidth, MapHeight);
            GameHost.Instance.Screen = GameScreen;

            // Destroy the default starting console that SadConsole created automatically because we're not using it.
            GameHost.Instance.DestroyDefaultStartingConsole();
        }
    }
}

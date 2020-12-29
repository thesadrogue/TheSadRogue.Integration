﻿using SadConsole;

namespace ExampleGame
{
    /// <summary>
    /// A tiny game to give examples of how to use GoRogue
    /// </summary>
    class Program
    {
        public const int Width = 80;
        public const int Height = 25;
        private const int MapWidth = 200;
        private const int MapHeight = 125;

        static void Main(/*string[] args*/)
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
            var ui = new GameUi(Width, Height, MapWidth, MapHeight);
            //GameHost.Instance.SetRendererStep("entitylite", );
            SadConsole.GameHost.Instance.Screen.Children.Add(ui);
        }
    }
}
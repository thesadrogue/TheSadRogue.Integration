using System;
using System.Collections.Generic;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SadConsole;
using SadRogue.Primitives;
using TheSadRogue.Integration;

namespace ExampleGame
{
    class Program
    {
        public const int Width = 80;
        public const int Height = 25;
        private const int _mapWidth = 400;
        private const int _mapHeight = 125;

        static void Main(string[] args)
        {
            SadConsole.Game.Create(Width, Height);
            SadConsole.Game.Instance.OnStart = Init;
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init()
        {
            var ui = new ExampleGameUi(Width, Height, _mapWidth, _mapHeight);
            ui.GenerateMap();
            SadConsole.GameHost.Instance.Screen.Children.Add(ui);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SadConsole; //we're using the extension method `Contains`
using SadRogue.Primitives;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Extensions;
using Region = GoRogue.MapGeneration.Region; //notice how region comes from GoRogue
using ScreenObject = SadConsole.ScreenObject; //ScreenObject is from SadConsole
using ScreenSurface = SadConsole.ScreenSurface; //ScreenSurface from SadConsole
using Console = SadConsole.Console; //Console referring to the SadConsole.Console

namespace ExampleGame
{
    /// <summary>
    /// A _very_ simple game UI
    /// </summary>
    public class GameUi : ScreenObject
    {
        public readonly RogueLikeMap Map;
        public readonly RogueLikeEntity PlayerCharacter;
        public readonly ScreenSurface MapWindow;
        public readonly Console MessageLogWindow;
        private readonly int _width, _height, _mapWidth, _mapHeight;
        public GameUi(in int width, in int height, in int mapWidth, in int mapHeight)
        {
            _width = width;
            _height = height;
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            MessageLogWindow = new SadConsole.Console(_width / 4, _height / 5);
            Map = GenerateMap();
            PlayerCharacter = GeneratePlayerCharacter();
            MapWindow = new SadConsole.ScreenSurface(_mapWidth, _mapHeight, Map.TerrainSurface.ToArray());
            
            Children.Add(MapWindow);
            foreach(ICellSurface glyphLayer in Map.Renderers)
                MapWindow.Children.Add(new ScreenSurface(_mapWidth, _mapHeight, glyphLayer.ToEnumerable().ToArray()));
        }

        private RogueLikeMap GenerateMap()
        {
            MapGenerator generator = new MapGenerator(_mapWidth, _mapHeight);
            return generator.GenerateMap();
        }

        private RogueLikeEntity GeneratePlayerCharacter()
        {
            RogueLikeEntity player = new RogueLikeEntity((_width/2,_height/2),1, layer: 1);
            RogueLikeComponent motionControl = new PlayerControlsComponent();
            player.AddComponent(motionControl);
            player.IsFocused = true;
            Map.AddEntity(player);
            return player;
        }
    }
}
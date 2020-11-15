using System.Linq;
using ExampleGame.MapGeneration;
using SadConsole; //we're using the extension method `Contains`
using TheSadRogue.Integration;
using TheSadRogue.Integration.Extensions;
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
            MessageLogWindow = new Console(_width / 4, _height / 5);
            Map = GenerateMap();
            PlayerCharacter = GeneratePlayerCharacter();
            MapWindow = new ScreenSurface(_mapWidth, _mapHeight, Map.TerrainSurface.ToArray());
            
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
            RogueLikeEntity player = new RogueLikeEntity((_mapWidth/2,_mapHeight/2),1, layer: 1);
            RogueLikeComponent motionControl = new PlayerControlsComponent();
            player.AddComponent(motionControl);
            player.IsFocused = true;
            Map.AddEntity(player);
            return player;
        }
    }
}
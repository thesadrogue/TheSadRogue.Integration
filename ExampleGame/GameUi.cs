using System.Linq;
using ExampleGame.MapGeneration;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Components;
using ScreenObject = SadConsole.ScreenObject; 
using ScreenSurface = SadConsole.ScreenSurface; 
using Console = SadConsole.Console;
using RogueLikeEntity = TheSadRogue.Integration.RogueLikeEntity;

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
        private readonly int _width;
        private readonly int _height;
        private readonly int _mapWidth;
        private readonly int _mapHeight;
        
        public GameUi(in int width, in int height, in int mapWidth, in int mapHeight)
        {
            _width = width;
            _height = height;
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            MessageLogWindow = new Console(width / 4, height / 5);
            Map = GenerateMap();
            MapWindow = new ScreenSurface(_mapWidth, _mapHeight, Map.TerrainCells.ToArray());
            Map.SetEntitySurface(MapWindow);

            PlayerCharacter = GeneratePlayerCharacter();
            
            Children.Add(MapWindow);
        }

        private RogueLikeMap GenerateMap()
        {
            var generator = new MapGenerator(_mapWidth, _mapHeight);
            return generator.GenerateMap();
        }

        private RogueLikeEntity GeneratePlayerCharacter()
        {
            var player = new RogueLikeEntity((_width/2,_height/2),1, layer: 1);
            var motionControl = new PlayerControlsComponent();
            player.AddComponent(motionControl);
            player.IsFocused = true;
            Map.AddEntity(player);
            return player;
        }
    }
}
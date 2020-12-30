using System.Linq;
using ExampleGame.MapGeneration;
using SadConsole;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Components;
// using TheSadRogue.Integration.Extensions;
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
            // MapWindow.Surface = Map.TerrainCells;
            // MapWindow.SadComponents.Add(Map.EntityManager);
            
            PlayerCharacter = GeneratePlayerCharacter();
            
            Children.Add(MapWindow);
            // foreach(ICellSurface glyphLayer in Map.Renderers)
            //     MapWindow.Children.Add(new ScreenSurface(glyphLayer));
        }

        private RogueLikeMap GenerateMap()
        {
            MapGenerator generator = new MapGenerator(_mapWidth, _mapHeight);
            return generator.GenerateMap();
        }

        private RogueLikeEntity GeneratePlayerCharacter()
        {
            RogueLikeEntity player = new RogueLikeEntity((_mapWidth/2,_mapHeight/2),1, layer: 1);
            RogueLikeComponentBase motionControl = new PlayerControlsComponent();
            player.AddComponent(motionControl);
            player.IsFocused = true;
            Map.AddEntity(player);
            return player;
        }
    }
}
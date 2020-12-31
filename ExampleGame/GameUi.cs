using System.Linq;
using GoRogue.MapGeneration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Components;
using TheSadRogue.Integration.MapGenerationSteps;
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
            var generator = new Generator(_mapWidth, _mapHeight);
            generator.AddStep(new CompositeGenerationStep(_mapWidth, _mapHeight));
            generator = generator.Generate();
            var generatedMap = generator.Context.GetFirst<ISettableGridView<bool>>();

            RogueLikeMap map = new RogueLikeMap(_mapWidth, _mapHeight, 4, Distance.Euclidean);
            int floorGlyph = '_';
            int wallGlyph = '#';
            
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    var here = (i, j);
                    int glyph = generatedMap[here] ? floorGlyph : wallGlyph;
                    map.SetTerrain(new RogueLikeEntity(here, glyph));
                }
            }

            return map;
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
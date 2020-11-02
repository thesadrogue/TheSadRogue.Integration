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
        private readonly Generator _mazeGenerator;
        private readonly MapGenerator _caveGenerator;
        public GameUi(in int width, in int height, in int mapWidth, in int mapHeight)
        {
            _width = width;
            _height = height;
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            _mazeGenerator = new Generator(width, height);
            _mazeGenerator.AddSteps(DefaultAlgorithms.DungeonMazeMapSteps(minRooms: 0, maxRooms: 0, maxCreationAttempts: 1, saveDeadEndChance: 70));
            _caveGenerator = new MapGenerator(width, height);
            MessageLogWindow = new SadConsole.Console(_width / 4, _height / 5);
            Map = GenerateMap();
            PlayerCharacter = GeneratePlayerCharacter();
            MapWindow = new SadConsole.ScreenSurface(_mapWidth, _mapHeight, Map.TerrainSurface.ToArray());
            
            Children.Add(MapWindow);
            foreach(ICellSurface glyphLayer in Map.Renderers)
                MapWindow.Children.Add(new ScreenSurface(_mapWidth, _mapHeight, glyphLayer.ToEnumerable().ToArray()));
            
        }

        private RogueLikeEntity GeneratePlayerCharacter()
        {
            RogueLikeEntity player = new RogueLikeEntity((_width/2,_height/2),1, layer: 1);
            RogueLikeComponent motionControl = new PlayerControlsComponent();
            player.AddComponent(motionControl);
            Map.AddEntity(player);
            return player;
        }

        public RogueLikeMap GenerateMap()
        {
            Random r = new Random();
            RogueLikeMap map = new RogueLikeMap(_mapWidth,_mapHeight, 31, Distance.Manhattan);
            foreach(var step in _mazeGenerator.GenerationSteps)
            {
                step.PerformStep(_mazeGenerator.Context);
            }
            _caveGenerator.MakeCaverns();
            IEnumerable<Region> regions = GenerateBackrooms();
            List<Region> mazeSections = new List<Region>();
            foreach (var region in regions)
            {
                int chance = r.Next(0, 100);
                if (chance < 40)
                    DrawRoom(map, region);
                else if (chance < 66)
                    DrawCave(map, region);
                else
                    mazeSections.Add(region);
            }

            foreach (var region in mazeSections)
            {
                DrawMaze(map, region);
            }
            return map;
        }

        private void DrawCave(RogueLikeMap map, Region region)
        {
            foreach (var point in region.Points)
            {
                if (map.Contains(point) && !_caveGenerator.IsOutOfBounds(point.X, point.Y))
                {
                    if (_caveGenerator.Map[point.X, point.Y])
                    {
                        map.SetTerrain(new RogueLikeEntity(point, '%', false, false));
                    }
                    else
                    {
                        map.SetTerrain(new RogueLikeEntity(point, ','));
                    }
                }
            }
        }

        private void DrawMaze(RogueLikeMap map, Region region)
        {
            var mazeView = _mazeGenerator.Context.GetFirst<IMapView<bool>>();
            foreach (var position in region.Points)
            {
                if (map.Contains(position) && mazeView.Contains(position))
                {
                    if (mazeView[position])
                    {
                        map.SetTerrain(new RogueLikeEntity(position, '.'));
                    }
                    else
                    {
                        map.SetTerrain(new RogueLikeEntity(position, '#', false, false));
                    }
                }
            }
        }

        private void DrawRoom(RogueLikeMap map, Region region)
        {
            foreach (var point in region.InnerPoints.Positions)
            {
                if (map.Contains(point))
                {
                    var terrain = new RogueLikeEntity(point, '.', true, true);
                    map.SetTerrain(terrain);
                }
            }

            foreach (var point in region.OuterPoints.Positions)
            {
                if (map.Contains(point))
                {
                    var terrain = new RogueLikeEntity(point, '#', false, false);
                    map.SetTerrain(terrain);
                }
            }

            foreach (var p in map.Positions().Where(p => map.GetTerrainAt(p) == null))
            {
                var terrain = new RogueLikeEntity(p, '.', true, true);
                map.SetTerrain(terrain);
            }
        }

        private IEnumerable<Region> GenerateBackrooms()
        {
            Random r = new Random();
            double rotationAngle = r.NextDouble() * 30;
            var scene = new RogueLikeMap(_mapWidth,_mapHeight, 31, Distance.Manhattan);
            int xOffset = r.Next(-15, 15) - 30;
            int yOffset = r.Next(-15, 15) - 30;
            
            Rectangle wholeMap = new Rectangle(xOffset, yOffset,_mapWidth + xOffset,_mapHeight +yOffset);

            Point center = (_width / 2, _height / 2);
            foreach (var room in wholeMap.BisectRecursive(8))
            {
                yield return Region.FromRectangle("room", room).Rotate(rotationAngle, center);
            }
        }
    }
}
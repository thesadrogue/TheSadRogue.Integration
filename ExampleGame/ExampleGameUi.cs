using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.MapViews;
using SadRogue.Primitives;
using TheSadRogue.Integration;
using Region = GoRogue.MapGeneration.Region; //notice how region comes from GoRogue
using ScreenObject = SadConsole.ScreenObject; //ScreenObject is from SadConsole
using ScreenSurface = SadConsole.ScreenSurface; //ScreenSurface from SadConsole
using Console = SadConsole.Console; //Console referring to the SadConsole.Console

namespace ExampleGame
{
    /// <summary>
    /// If I were to make the simplest Game Screen I could...
    /// </summary>
    public class ExampleGameUi : ScreenObject
    {
        public RogueLikeMap Map;
        public ScreenSurface MapWindow;
        public Console MessageLogWindow;
        private readonly int _width, _height, _mapWidth, _mapHeight;
        public ExampleGameUi(in int width, in int height, in int mapWidth, in int mapHeight)
        {
            _width = width;
            _height = height;
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
        }

        public virtual void GenerateMap()
        {
            Map = GenerateBackrooms();
            MessageLogWindow = new SadConsole.Console(_width / 4, _height);
            MapWindow = new SadConsole.ScreenSurface(_mapWidth, _mapHeight, Map.RenderingCellData);
            Children.Add(MapWindow);
        }

        private RogueLikeMap GenerateBackrooms()
        {
            Random r = new Random();
            double rotationAngle = r.NextDouble() * 20;
            var scene = new RogueLikeMap(_mapWidth,_mapHeight, 31, Distance.Manhattan);
            int xOffset = r.Next(-13, 13) - 13;
            int yOffset = r.Next(-13, 13) - 13;
            
            Rectangle wholeMap = new Rectangle(xOffset, yOffset,_mapWidth + xOffset,_mapHeight +yOffset);
            IEnumerable<Rectangle> rooms = wholeMap.BisectRecursive(8);

            Point center = (_mapWidth/2, _mapHeight/2);
            
            foreach (var room in rooms)
            {
                Region region = Region.FromRectangle("room", room).Rotate(rotationAngle, center);
                foreach (var point in region.InnerPoints.Positions)
                {
                    if (scene.Contains(point))
                    {
                        var terrain = new RogueLikeEntity(point, '.', true, true);
                        scene.SetTerrain(terrain);
                    }
                }
                foreach (var point in region.OuterPoints.Positions)
                {
                    if (scene.Contains(point))
                    {
                        var terrain = new RogueLikeEntity(point, '#', false, false);
                        scene.SetTerrain(terrain);
                    }
                }
            }

            foreach (var p in scene.Positions().Where(p => scene.GetTerrainAt(p) == null))
            {
                var terrain = new RogueLikeEntity(p, '.', true, true);
                scene.SetTerrain(terrain);
            }
            return scene;
        }
    }
}
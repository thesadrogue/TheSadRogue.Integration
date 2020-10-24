using System;
using System.Collections.Generic;
using System.Linq;
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
            
            foreach(ICellSurface glyphLayer in Map.RenderingGlyphs)
                MapWindow.Children.Add(new ScreenSurface(_mapWidth, _mapHeight, glyphLayer.ToEnumerable().ToArray()));
            
            Children.Add(MapWindow);
        }

        private RogueLikeEntity GeneratePlayerCharacter()
        {
            RogueLikeEntity player = new RogueLikeEntity((_width/2,_height/2),1, layer: 1);
            RogueLikeComponent motionControl = new PlayerControlsComponent();
            player.AddComponent(motionControl);
            Map.AddEntity(player);
            return player;
        }

        private RogueLikeMap GenerateMap()
        {
            Random r = new Random();
            double rotationAngle = r.NextDouble() * 30;
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
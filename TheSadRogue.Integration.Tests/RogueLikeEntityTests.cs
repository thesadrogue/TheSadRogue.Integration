using SadRogue.Primitives;
using TheSadRogue.Integration.Components;
using Xunit;

namespace TheSadRogue.Integration.Tests
{
    public class RogueLikeEntityTests
    {
        #region init
        [Fact]
        public void NewFromColorsAndGlyphTest()
        {
            RogueLikeEntity entity = new RogueLikeEntity(Color.Chartreuse, Color.Salmon, '1', 0);
            
            Assert.Equal(Color.Chartreuse, entity.Appearance.Foreground);
            Assert.Equal(Color.Salmon, entity.Appearance.Background);
            Assert.Equal('1', entity.Appearance.Glyph);
            Assert.Equal(new Point(0,0), entity.Position);
            Assert.True(entity.IsWalkable); //the default
            Assert.True(entity.IsTransparent); //the default
        }
        
        [Fact]
        public void NewFromPositionAndGlyphTest()
        {
            RogueLikeEntity entity = new RogueLikeEntity((1,1), 2);
            Assert.Equal(Color.White, entity.Appearance.Foreground);
            Assert.Equal(Color.Black, entity.Appearance.Background);
            Assert.Equal(2, entity.Appearance.Glyph);
            Assert.Equal(new Point(1,1), entity.Position);
        }
        
        [Fact]
        public void NewFromPositionColorAndGlyphTest()
        {
            RogueLikeEntity entity = new RogueLikeEntity((1,3), Color.Cyan, 2);
            Assert.Equal(Color.Cyan, entity.Appearance.Foreground);
            Assert.Equal(Color.Black, entity.Appearance.Background);
            Assert.Equal(2, entity.Appearance.Glyph);
            Assert.Equal(new Point(1,3), entity.Position);
        }
        [Fact]
        public void NewFromColorGlyphAndLayerTest()
        {
            RogueLikeEntity entity = new RogueLikeEntity(Color.Black, Color.Cyan, 2, 1);
            Assert.Equal(Color.Cyan, entity.Appearance.Background);
            Assert.Equal(Color.Black, entity.Appearance.Foreground);
            Assert.Equal(2, entity.Appearance.Glyph);
            Assert.Equal(1, entity.Layer);
        }
        #endregion
        #region motion
        [Fact]
        public void OnMapChangedTest()
        {
            RogueLikeEntity entity = new RogueLikeEntity(Color.White, Color.Black, 1, 1);
            RogueLikeMap map = new RogueLikeMap(64, 64, 4, Distance.Manhattan);
            
            map.AddEntity(entity);
            
            Assert.NotNull(entity.CurrentMap);
            Assert.Equal(map.Width, entity.CurrentMap.Width);
            Assert.Equal(map.Height, entity.CurrentMap.Height);

            var newMap = new RogueLikeMap(20, 20, 3, Distance.Euclidean);
            newMap.AddEntity(entity);
            
            Assert.Equal(newMap.Width, entity.CurrentMap.Width);
            Assert.Equal(newMap.Height, entity.CurrentMap.Height);
        }

        [Fact]
        public void CanMoveTest()
        {
            var entity = new RogueLikeEntity((1, 1), 1, layer: 1);
            var map = new RogueLikeMap(3, 3, 2, Distance.Chebyshev);
            
            map.SetTerrain(new RogueLikeEntity((0,0), '.', true));
            map.SetTerrain(new RogueLikeEntity((0,1), '#', false));
            map.SetTerrain(new RogueLikeEntity((0,2), '#', false));
            map.SetTerrain(new RogueLikeEntity((1,0), '.', true));
            map.SetTerrain(new RogueLikeEntity((1,1), '.', true));
            map.SetTerrain(new RogueLikeEntity((1,2), '#', false));
            map.SetTerrain(new RogueLikeEntity((2,0), '.', true));
            map.SetTerrain(new RogueLikeEntity((2,1), '.', true));
            map.SetTerrain(new RogueLikeEntity((2,2), '#', false));
            
            map.AddEntity(entity);

            Assert.True(entity.CanMove((0, 0)));
            Assert.True(entity.CanMove((1, 0)));
            Assert.True(entity.CanMove((2, 0)));
            Assert.True(entity.CanMove((1, 1)));
            Assert.True(entity.CanMove((2, 1)));
            Assert.False(entity.CanMove((0, 1)));
            Assert.False(entity.CanMove((0, 2)));
            Assert.False(entity.CanMove((1, 2)));
            Assert.False(entity.CanMove((2, 2)));
        }
        [Fact]
        public void CanMoveInTest()
        {
            var entity = new RogueLikeEntity((1, 1), 1, layer: 1);
            var map = new RogueLikeMap(3, 3, 2, Distance.Chebyshev);

            map.SetTerrain(new RogueLikeEntity((0,0), '.', true));
            map.SetTerrain(new RogueLikeEntity((0,1), '#', false));
            map.SetTerrain(new RogueLikeEntity((0,2), '#', false));
            map.SetTerrain(new RogueLikeEntity((1,0), '.', true));
            map.SetTerrain(new RogueLikeEntity((1,1), '.', true));
            map.SetTerrain(new RogueLikeEntity((1,2), '#', false));
            map.SetTerrain(new RogueLikeEntity((2,0), '.', true));
            map.SetTerrain(new RogueLikeEntity((2,1), '.', true));
            map.SetTerrain(new RogueLikeEntity((2,2), '#', false));
            
            map.AddEntity(entity);

            Assert.True(entity.CanMoveIn(Direction.UpLeft));
            Assert.True(entity.CanMoveIn(Direction.Up));
            Assert.True(entity.CanMoveIn(Direction.UpRight));
            Assert.True(entity.CanMoveIn(Direction.Right));
            Assert.True(entity.CanMoveIn(Direction.None));
            Assert.False(entity.CanMoveIn(Direction.Left));
            Assert.False(entity.CanMoveIn(Direction.DownLeft));
            Assert.False(entity.CanMoveIn(Direction.Down));
            Assert.False(entity.CanMoveIn(Direction.DownRight));
        }
        #endregion
        #region components

        [Fact]
        public void HasComponentTest()
        {
            var entity = new RogueLikeEntity((1, 1), 1);
            Assert.False(entity.HasComponent<PlayerControlsComponent>());
            
            entity.AddComponent(new PlayerControlsComponent());
            Assert.True(entity.HasComponent<PlayerControlsComponent>());
        }
        [Fact]
        public void AddComponentTest()
        {
            var entity = new RogueLikeEntity((1, 1), 1);
            Assert.False(entity.HasComponent<PlayerControlsComponent>());
            
            entity.AddComponent(new PlayerControlsComponent());
            Assert.True(entity.HasComponent<PlayerControlsComponent>());
        }
        #endregion
    }
}
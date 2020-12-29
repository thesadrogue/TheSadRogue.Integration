using SadRogue.Primitives;
using Xunit;

namespace TheSadRogue.Integration.Tests
{
    public class InitializationTests
    {
        private RogueLikeEntity _entity;
        
        [Fact]
        public void NewFromColorsAndGlyphTest()
        {
            _entity = new RogueLikeEntity(Color.Chartreuse, Color.Salmon, '1', 0);
            
            Assert.Equal(Color.Chartreuse, _entity.Appearance.Foreground);
            Assert.Equal(Color.Salmon, _entity.Appearance.Background);
            Assert.Equal('1', _entity.Appearance.Glyph);
            Assert.Equal(new Point(0,0), _entity.Position);
            Assert.True(_entity.IsWalkable); //the default
            Assert.True(_entity.IsTransparent); //the default
        }
        
        [Fact]
        public void NewFromPositionAndGlyphTest()
        {
            _entity = new RogueLikeEntity((1,1), 2);
            Assert.Equal(Color.White, _entity.Appearance.Foreground);
            Assert.Equal(Color.Black, _entity.Appearance.Background);
            Assert.Equal(2, _entity.Appearance.Glyph);
            Assert.Equal(new Point(1,1), _entity.Position);
        }
        
        [Fact]
        public void NewFromPositionColorAndGlyphTest()
        {
            _entity = new RogueLikeEntity((1,3), Color.Cyan, 2);
            Assert.Equal(Color.Cyan, _entity.Appearance.Foreground);
            Assert.Equal(Color.Black, _entity.Appearance.Background);
            Assert.Equal(2, _entity.Appearance.Glyph);
            Assert.Equal(new Point(1,3), _entity.Position);
        }
    }
}
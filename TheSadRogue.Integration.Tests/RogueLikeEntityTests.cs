using SadRogue.Primitives;
using Xunit;

namespace TheSadRogue.Integration.Tests.RogueLikeEntity
{
    public class InitializationTests
    {
        private TheSadRogue.Integration.RogueLikeEntity entity;
        
        [Fact]
        public void NewFromColorsAndGlyphTest()
        {
            entity = new TheSadRogue.Integration.RogueLikeEntity(Color.Chartreuse, Color.Salmon, '1', 1);
            
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
            entity = new TheSadRogue.Integration.RogueLikeEntity((1,1), 2);
            Assert.Equal(Color.White, entity.Appearance.Foreground);
            Assert.Equal(Color.Black, entity.Appearance.Background);
            Assert.Equal(2, entity.Appearance.Glyph);
            Assert.Equal(new Point(1,1), entity.Position);
        }
        
        [Fact]
        public void NewFromPositionColorAndGlyphTest()
        {
            entity = new TheSadRogue.Integration.RogueLikeEntity((1,3), Color.Cyan, 2);
            Assert.Equal(Color.Cyan, entity.Appearance.Foreground);
            Assert.Equal(Color.Black, entity.Appearance.Background);
            Assert.Equal(2, entity.Appearance.Glyph);
            Assert.Equal(new Point(1,3), entity.Position);
        }
    }
}
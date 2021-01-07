using SadConsole;
using SadRogue.Primitives;
using TheSadRogue.Integration.Components;
using Xunit;

namespace TheSadRogue.Integration.Tests
{
    public class RogueLikeEntityTests
    {
        [Fact]
        public void NewFromColorsAndGlyphTest()
        {
            var entity = new RogueLikeEntity((0,0), Color.Chartreuse, Color.Salmon, '1');
            
            Assert.Equal(Color.Chartreuse, entity.Appearance.Foreground);
            Assert.Equal(Color.Salmon, entity.Appearance.Background);
            Assert.Equal('1', entity.Appearance.Glyph);
            Assert.Equal(new Point(0,0), entity.Position);
            Assert.True(entity.IsWalkable); //the default
            Assert.False(entity.IsTransparent); //the default
        }
        
        [Fact]
        public void NewFromPositionAndGlyphTest()
        {
            var entity = new RogueLikeEntity((1,1), 2);
            Assert.Equal(Color.White, entity.Appearance.Foreground);
            Assert.Equal(Color.Black, entity.Appearance.Background);
            Assert.Equal(2, entity.Appearance.Glyph);
            Assert.Equal(new Point(1,1), entity.Position);
        }
        
        [Fact]
        public void NewFromPositionColorAndGlyphTest()
        {
            var entity = new RogueLikeEntity((1,3), Color.Cyan, 2);
            Assert.Equal(Color.Cyan, entity.Appearance.Foreground);
            Assert.Equal(Color.Black, entity.Appearance.Background);
            Assert.Equal(2, entity.Appearance.Glyph);
            Assert.Equal(new Point(1,3), entity.Position);
        }
        [Fact]
        public void AddComponentTest()
        {
            var entity = new RogueLikeEntity((1,3), Color.Cyan, 2);
            var component = new PlayerControlsComponent();
            
            Assert.Equal(4, component.Motions.Count);
            Assert.Empty(component.Actions);
            
            entity.AddComponent(component);
            
            Assert.Single(entity.SadComponents);
            Assert.Single(entity.GoRogueComponents);
        }
    }
}
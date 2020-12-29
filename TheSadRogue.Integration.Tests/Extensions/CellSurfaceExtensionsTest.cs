using System.Linq;
using SadConsole;
using SadRogue.Primitives;
using TheSadRogue.Integration.Extensions;
using Xunit;

namespace TheSadRogue.Integration.Tests.Extensions
{
    public class CellSurfaceExtensionsTest
    {
        [Fact]
        public void ToEnumerableTest()
        {
            ColoredGlyph[] cells = 
            {
                new ColoredGlyph(Color.White, Color.Black, '#'), 
                new ColoredGlyph(Color.White, Color.White, ' '), 
                new ColoredGlyph(Color.White, Color.Black, '&'), 
                new ColoredGlyph(Color.Cyan, Color.White, '#'), 
                new ColoredGlyph(Color.Cyan, Color.Black, ' '), 
                new ColoredGlyph(Color.Cyan, Color.White, '&'), 
                new ColoredGlyph(Color.LightGray, Color.Black, '#'), 
                new ColoredGlyph(Color.LightGray, Color.White, ' '), 
                new ColoredGlyph(Color.LightGray, Color.Black, '&'), 
            };
            ICellSurface surface = new CellSurface(3,3, cells);
            
            var answer = surface.ToEnumerable();
            
            Assert.True(answer.Count() == 9, "Answer did not contain 9 items.");
            Assert.Equal(3, answer.Count(glyph => glyph.Foreground == Color.Cyan));
            Assert.Equal(3, answer.Count(glyph => glyph.Foreground == Color.White));
            Assert.Equal(3, answer.Count(glyph => glyph.Foreground == Color.LightGray));
            Assert.Equal(5, answer.Count(glyph => glyph.Background == Color.Black));
            Assert.Equal(4, answer.Count(glyph => glyph.Background == Color.White));
            
            Assert.Equal(3, answer.Count(glyph => glyph.Glyph == '#'));
            Assert.Equal(3, answer.Count(glyph => glyph.Glyph == ' '));
            Assert.Equal(3, answer.Count(glyph => glyph.Glyph == '&'));
            
        }
    }
}
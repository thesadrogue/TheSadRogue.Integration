using SadConsole;
using SadRogue.Primitives;
using Xunit;

namespace TheSadRogue.Integration.Tests.RogueLikeEntity.Extensions
{
    public class CellSurfaceExtensionsTest
    {
        [Fact]
        public void ToEnumerableTest()
        {
            ColoredGlyph[] cells = new[]
            {
                new ColoredGlyph(Color.White, Color.Black, '#'), 
                new ColoredGlyph(Color.White, Color.Black, '#'), 
                new ColoredGlyph(Color.White, Color.Black, '#'), 
                new ColoredGlyph(Color.White, Color.Black, '#'), 
                new ColoredGlyph(Color.White, Color.Black, ' '), 
                new ColoredGlyph(Color.White, Color.Black, '&'), 
                new ColoredGlyph(Color.White, Color.Black, '&'), 
                new ColoredGlyph(Color.White, Color.Black, '&'), 
                new ColoredGlyph(Color.White, Color.Black, '&'), 
            };
            ICellSurface surface = new CellSurface(3,3);
        }
    }
}
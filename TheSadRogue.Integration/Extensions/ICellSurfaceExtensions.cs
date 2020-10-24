using System.Collections.Generic;
using SadConsole;

namespace TheSadRogue.Integration.Extensions
{
    public static class ICellSurfaceExtensions
    {
        public static IEnumerable<ColoredGlyph> ToEnumerable(this ICellSurface self)
        {
            for (int i = 0; i < self.BufferHeight; i++)
            {
                for (int j = 0; j < self.BufferWidth; j++)
                {
                    yield return self[j, i];
                }
            }
        }
    }
}
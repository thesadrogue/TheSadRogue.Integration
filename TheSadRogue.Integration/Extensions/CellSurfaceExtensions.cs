using System.Collections.Generic;
using SadConsole;

namespace TheSadRogue.Integration.Extensions
{
    /// <summary>
    /// Extensions for ICellSurface
    /// </summary>
    public static class CellSurfaceExtensions
    {
        /// <summary>
        /// Turns an ICellSurface into an IEnumerable of ColoredGlyph
        /// </summary>
        /// <param name="self"></param>
        /// <returns>The ColoredGlyphs in an IEnumerable</returns>
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
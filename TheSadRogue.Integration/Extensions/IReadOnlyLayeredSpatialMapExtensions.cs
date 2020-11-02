using System.Collections.Generic;
using GoRogue.GameFramework;
using GoRogue.SpatialMaps;
using SadConsole;
using SadRogue.Primitives;
using Layer = SadConsole.LayeredScreenSurface.Layer;

namespace TheSadRogue.Integration.Extensions
{
    public static class IReadOnlyLayeredSpatialMapExtensions
    {
        public static IEnumerable<ICellSurface> ToCellSurfaces(this IReadOnlyLayeredSpatialMap<IGameObject> self, int width, int height)
        {
            foreach (var layer in self.Layers)
            {
                CellSurface surface = new CellSurface(width, height);

                foreach (RogueLikeEntity entity in layer.Items)
                {
                    var glyph = entity.Glyph;
                    surface.SetGlyph(entity.Position.X, entity.Position.Y, glyph.Glyph, glyph.Foreground, glyph.Background);
                }

                yield return surface;
            }
        }
    }
}
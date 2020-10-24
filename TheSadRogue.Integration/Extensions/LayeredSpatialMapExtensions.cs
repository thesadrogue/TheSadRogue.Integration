using System.Collections.Generic;
using System.Linq;
using GoRogue.SpatialMaps;
using SadConsole;
using SadRogue.Primitives;

namespace TheSadRogue.Integration.Extensions
{
    public static class LayeredSpatialMapExtensions
    {
        public static IEnumerable<CellSurface> ToCellSurfaces(this LayeredSpatialMap<RogueLikeEntity> self, int width, int height)
        {
            ColoredGlyph[] blankGlyphs = new ColoredGlyph[width * height];
            for (int j = 0; j < blankGlyphs.Length; j++)
            {
                blankGlyphs[j] = new ColoredGlyph(Color.Transparent, Color.Transparent, 0);
            }
            
            List<CellSurface> surfaces = new List<CellSurface>();
            for (int i = 0; i < self.Layers.Count(); i++)
            {
                CellSurface layer = new CellSurface(width, height, blankGlyphs);

                foreach (var entity in self.Layers.ToArray()[i]) //hacky
                {
                    layer.SetGlyph(entity.Position.X, entity.Position.Y, entity.Item.Glyph.Glyph);
                }

                surfaces.Add(layer);
            }

            return surfaces;
        }
    }
}
using System.Collections.Generic;
using GoRogue.GameFramework;
using GoRogue.SpatialMaps;
using SadConsole;

namespace TheSadRogue.Integration.Extensions
{
    /// <summary>
    /// Extensions for IReadOnlyLayeredSpatialMap
    /// </summary>
    public static class ReadOnlyLayeredSpatialMapExtensions
    {
        /// <summary>
        /// Turns an IReadOnlyLayeredSpatialMap into an enumerable of CellSurfaces
        /// </summary>
        /// <param name="self">The spatial map to transform</param>
        /// <param name="width">The desired width of the cell surfaces</param>
        /// <param name="height">The desired height of the cell surfaces</param>
        /// <returns>A collection of CellSurfaces that represent where all entities are placed</returns>
        public static IEnumerable<ICellSurface> ToCellSurfaces(this IReadOnlyLayeredSpatialMap<IGameObject> self, int width, int height)
        {
            foreach (var layer in self.Layers)
            {
                CellSurface surface = new CellSurface(width, height);

                foreach (RogueLikeEntity entity in layer.Items)
                {
                    surface.SetGlyph(entity.Position.X, entity.Position.Y, entity.Glyph, entity.Foreground, entity.Background);
                }

                yield return surface;
            }
        }
    }
}
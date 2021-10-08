using System;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;

namespace TheSadRogue.Integration.Templates.MonoGame
{
    /// <summary>
    /// RogueLikeMap class that simplifies constructor and wraps map layers into a convenient, type-safe, customizable
    /// enumeration. Add/remove values from the enum as you like; the map will update accordingly to reflect number and order.
    /// </summary>
    internal class MyGameMap : RogueLikeMap
    {
        public enum Layer
        {
            Terrain = 0,
            Monsters,
            Items
        };

        public MyGameMap(int width, int height, DefaultRendererParams? defaultRendererParams)
            : base(width, height, defaultRendererParams, Enum.GetValues<Layer>().Length - 1, Distance.Chebyshev)
        { }
    }
}

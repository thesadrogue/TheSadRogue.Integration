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
        // CUSTOMIZATION: Edit map layers here as desired; however ensure that Terrain stays as 0 to match GoRogue's
        // definition of the terrain layer.
        public enum Layer
        {
            Terrain = 0,
            Monsters,
            Items
        }

        // CUSTOMIZATION: Change the distance from Distance.Chebyshev to whatever is desired for your game.  By default,
        // this will affect the FOV shape as well as the distance calculation used for AStar pathfinding on the Map.
        public MyGameMap(int width, int height, DefaultRendererParams? defaultRendererParams)
            : base(width, height, defaultRendererParams, Enum.GetValues<Layer>().Length - 1, Distance.Chebyshev)
        { }
    }
}

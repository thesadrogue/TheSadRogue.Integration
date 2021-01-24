using System;
using GoRogue.Components;
using GoRogue.GameFramework;
using SadConsole;
using SadRogue.Primitives;

namespace TheSadRogue.Integration
{
    /// <summary>
    /// A GameObject with a ColoredGlyph.
    /// </summary>
    /// <remarks>
    /// This class is designed to be very light and efficient.
    /// Use it for terrain.
    /// </remarks>
    public class RogueLikeCell : GameObject
    {
        /// <summary>
        /// The ColoredGlyph of this cell
        /// </summary>
        public TerrainAppearance Appearance { get; }

        /// <summary>
        /// Creates a new RogueLikeCell
        /// </summary>
        /// <param name="position">Where the Cell is located</param>
        /// <param name="foreground">The Foreground color of the Cell</param>
        /// <param name="background">The Foreground color of the Cell</param>
        /// <param name="glyph">The symbol of the Cell</param>
        /// <param name="layer">The map layer to which this Cell is added</param>
        /// <param name="walkable">Whether the Cell is considered in Collision Detection</param>
        /// <param name="transparent">Whether the Cell is considered in Field-of-View algorithms</param>
        /// <param name="idGenerator">The function which produces the unique ID for this Cell</param>
        /// <param name="customComponentContainer">Accepts a custom collection</param>
        public RogueLikeCell(Point position, Color foreground, Color background, int glyph, int layer,
            bool walkable = true, bool transparent = true, Func<uint>? idGenerator = null,
            ITaggableComponentCollection? customComponentContainer = null)
            : base(position, layer, walkable, transparent, idGenerator, customComponentContainer)
        {
            Appearance = new TerrainAppearance(this, foreground, background, glyph);
        }
        /// <summary>
        /// Creates a new RogueLikeCell
        /// </summary>
        /// <param name="position">Where the Cell is located</param>
        /// <param name="appearance">The Foreground color of the Cell</param>
        /// <param name="layer">The map layer to which this Cell is added</param>
        /// <param name="walkable">Whether the Cell is considered in Collision Detection</param>
        /// <param name="transparent">Whether the Cell is considered in Field-of-View algorithms</param>
        /// <param name="idGenerator">The function which produces the unique ID for this Cell</param>
        /// <param name="customComponentContainer">Accepts a custom collection</param>
        public RogueLikeCell(Point position, ColoredGlyph appearance, int layer, bool walkable = true, bool transparent = true,
            Func<uint>? idGenerator = null, ITaggableComponentCollection? customComponentContainer = null)
            : base(position, layer, walkable, transparent, idGenerator, customComponentContainer)
        {
            Appearance = new TerrainAppearance(this, appearance);
        }
    }
}

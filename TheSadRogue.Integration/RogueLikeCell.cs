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
        public ColoredGlyph? Appearance { get; }
        
        /// <summary>
        /// Fired when the Appearance is changed.
        /// </summary>
        public event EventHandler? AppearanceChanged;
        
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
            Appearance = new ColoredGlyph(foreground, background, glyph);
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
        public RogueLikeCell(Point position, ColoredGlyph glyph, int layer, bool walkable = true, bool transparent = true, 
            Func<uint>? idGenerator = null, ITaggableComponentCollection? customComponentContainer = null) 
            : base(position, layer, walkable, transparent, idGenerator, customComponentContainer)
        {
            Appearance = glyph;
        }

        /// <summary>
        /// Sets the glyph of the appearance of the object.
        /// </summary>
        /// <param name="glyph"/>
        public void SetGlyph(int glyph)
        {
            if (Appearance.Glyph != glyph)
            {
                Appearance.Glyph = glyph;
                AppearanceChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Sets the foreground color of the glyph for the object.
        /// </summary>
        /// <param name="foreground"/>
        public void SetForeground(Color foreground)
        {
            if (Appearance.Foreground != foreground)
            {
                Appearance.Foreground = foreground;
                AppearanceChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Sets the foreground color of the glyph for the object.
        /// </summary>
        /// <param name="background"/>
        public void SetBackground(Color background)
        {
            if (Appearance.Background != background)
            {
                Appearance.Background = background;
                AppearanceChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
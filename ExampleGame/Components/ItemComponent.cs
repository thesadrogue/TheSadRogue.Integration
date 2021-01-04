using SadConsole;
using SadRogue.Primitives;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Components;

namespace ExampleGame.Components
{   
    /// <summary>
    /// Which "slot" on our character an item  goes into.
    /// </summary>
    /// <remarks>Only two slots, Wielded and Worn, exist in this very simple example game.</remarks>
    public enum InventorySlot
    {
        Wielded, // wielded in the hand
        Worn,   // worn on the body
    }
    /// <summary>
    /// The Component given to items
    /// </summary>
    /// <remarks>Any RogueLikeEntity with this component is an Item</remarks>
    public class ItemComponent : RogueLikeComponentBase
    {
        public string Name;
        public InventorySlot Slot { get; }
        public ColoredGlyph Appearance { get; }
        public int Modifier { get; }

        public ItemComponent(string name, InventorySlot slot, int modifier, Color foreground, int glyph) 
            : base(false, false, false, false, 3)
        {
            Name = name;
            Slot = slot;
            Modifier = modifier;
            Appearance = new ColoredGlyph(foreground, Color.Black, glyph);
            ((RogueLikeEntity) Parent!).Appearance = Appearance;
        }
    }
}
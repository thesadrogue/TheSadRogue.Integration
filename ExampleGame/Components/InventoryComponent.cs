using System.Collections.Generic;
using ExampleGame.Items;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Components;

namespace ExampleGame.Components
{
    /// <summary>
    /// The component given to any RogueLikeEntity which has an inventory
    /// </summary>
    /// <remarks>Any creature with this component has an inventory</remarks>
    public class InventoryComponent : RogueLikeComponentBase
    {
        public ItemComponent? Wielded;
        public ItemComponent? Worn;
        public ItemComponent? Pack;
        
        // quest items - needs 100 in each in order to summon the boss
        public int HasSnow { get; private set; }
        public int HasBlood  { get; private set; }
        public int HasBone { get; private set; }
        public int HasSlime { get; private set; }
        
        public InventoryComponent(ItemComponent wielded = null, ItemComponent worn = null, ItemComponent pack = null) : base(false, false, false, false, 2)
        {
            Wielded = wielded;
            Worn = worn;
            Pack = pack;
        }
    }
}
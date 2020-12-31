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
        
        // quest items - needs 100 in each in order to summon the boss
        public int Snow { get; private set; }
        public int Bone { get; private set; }
        public int Slime { get; private set; }
        
        public InventoryComponent() : base(false, false, false, false, 2)
        {
            Wielded = null;
            Worn = null;
            Snow = 0;
            Bone = 0;
            Slime = 0;
        }

        public void CollectSlime(int amount) => Slime += amount;
        public void CollectBone(int amount) => Bone += amount;
        public void CollectSnow(int amount) => Snow += amount;

        public void EquipItem(ItemComponent item)
        {
            if (item.Slot == InventorySlot.Wielded)
            {
                RemoveCurrentlyWieldedItem();
                Wielded = item;
            }
            
            if (item.Slot == InventorySlot.Worn)
            {
                RemoveCurrentlyWornItem();
                Worn = item;
            }
        }

        private void RemoveCurrentlyWieldedItem()
        {
            //todo - drop item
        }
        private void RemoveCurrentlyWornItem()
        {
            //todo - drop item
        }
    }
}
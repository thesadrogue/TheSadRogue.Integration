namespace ExampleGame.Items
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
}
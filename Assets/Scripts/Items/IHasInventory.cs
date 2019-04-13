namespace Items
{
    /// <summary>
    ///     An entity that implements this method has an inventory.
    /// </summary>
    public interface IHasInventory
    {
        Inventory Inventory { get; }
    }
}
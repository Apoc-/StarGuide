using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Items
{
    /// <summary>
    ///     An inventory contains items.
    /// </summary>
    public class Inventory
    {
        private readonly Dictionary<int, Item> InventoryItems = new Dictionary<int, Item>();

        public Inventory(int inventorySlots)
        {
            Debug.Assert(inventorySlots > 0, "Inventory Size must be > 0");

            InventorySlots = inventorySlots;
        }

        public int InventorySlots { get; }

        public Item GetItem(int itemPos)
        {
            InventoryItems.TryGetValue(itemPos, out var result);
            return result;
        }

        public bool RemoveItem(int itemPos)
        {
            return InventoryItems.Remove(itemPos);
        }

        public bool RemoveItem(Item item)
        {
            var itemPosition = GetItemPosition(item);
            return itemPosition.HasValue && RemoveItem(itemPosition.Value);
        }

        public void AddItem(Item item)
        {
            var pos = GetLowestPossiblePosition();
            if (pos.HasValue) InventoryItems.Add(pos.Value, item);
        }

        public int? GetItemPosition(Item t)
        {
            return InventoryItems
                .Where(kvp => kvp.Value.Equals(t))
                .Select(kvp => kvp.Key)
                .FirstOrDefault();
        }

        public bool MoveItem(Item item, int newPos)
        {
            var itemPosition = GetItemPosition(item);
            return itemPosition.HasValue && MoveItem(itemPosition.Value, newPos);
        }

        public bool MoveItem(int oldPos, int newPos)
        {
            if (!InventoryItems.ContainsKey(oldPos))
                return false;
            if (InventoryItems.ContainsKey(newPos))
                return false;

            InventoryItems.Add(newPos, InventoryItems[oldPos]);
            InventoryItems.Remove(oldPos);

            return true;
        }

        protected int? GetLowestPossiblePosition()
        {
            var invKeys = InventoryItems.Keys.OrderBy(i => i).ToList();
            var lowestPossiblePosition = Enumerable.Range(0, invKeys.Last() + 1).Except(invKeys).First();
            if (lowestPossiblePosition >= InventorySlots)
                return null;
            return lowestPossiblePosition;
        }

        public bool MoveItemToOtherInventory(int itemPos, Inventory otherInventory)
        {
            if (!otherInventory.HasSpaceForItem()) return false;

            otherInventory.AddItem(GetItem(itemPos));
            return RemoveItem(itemPos);
        }

        public bool MoveItemToOtherInventory(Item item, Inventory otherInventory)
        {
            var itemPosition = GetItemPosition(item);
            return itemPosition.HasValue && MoveItemToOtherInventory(itemPosition.Value, otherInventory);
        }

        public bool HasSpaceForItem()
        {
            return InventoryItems.Count < InventorySlots;
        }
    }
}
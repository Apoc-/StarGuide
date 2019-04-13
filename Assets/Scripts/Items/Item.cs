using System;

namespace Items
{
    [Serializable]
    public class Item
    {
        public Item(ItemType type, int amount = 0, int flag = 0)
        {
            Type = type;
            Amount = amount;
            Flags = flag;
        }

        public Item(Item item)
        {
            Type = item.Type;
            Amount = item.Amount;
        }
        
        public int Flags { get; protected set; }
        
        public ItemType Type { get; protected set; }
        public int Amount { get; protected set; }
    }
}
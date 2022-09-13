using System;
using InventorySystem.Items;

namespace InventorySystem.Core
{
    public interface ISlotContainer  
    {
        bool IsFull { get; }
        bool IsEmpty { get; }
        Item Item { get; }
        Type ItemType { get; }
        int Amount { get; set; }
        int Capacity { get; }
        bool IsEquipped { get; set; }
        void Clear();
        void SetItem(IItemContainer itemContainer, int amount);
    }
}
using System;
using InventorySystem.Items;
using InventorySystem.Items.Core;

namespace InventorySystem.Core
{
    public interface ISlotContainer  
    {
        bool IsFull { get; }
        bool IsEmpty { get; }
        Item Item { get; }
        Type ItemGetType { get; }
        int Amount { get; set; }
        int Capacity { get; }
        bool IsEquipped { get; set; }
        void Clear();
        bool AllRequirementsChecked(Item item, int amount);
        void SetItem(IItem item, int amount);
    }
}
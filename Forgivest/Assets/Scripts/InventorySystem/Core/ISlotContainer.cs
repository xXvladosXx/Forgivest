using System;

namespace InventorySystem.Core
{
    public interface ISlotContainer  
    {
        bool IsFull { get; }
        bool IsEmpty { get; }
        
        IItemContainer Item { get; }
        Type ItemType { get; }
        int Amount { get; }
        int Capacity { get; }

        void Clear();
    }
}
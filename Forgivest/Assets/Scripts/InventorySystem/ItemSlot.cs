using System;
using InventorySystem.Core;

namespace InventorySystem
{
    
    public class ItemSlot : ISlotContainer
    {
        
        public bool IsFull => Amount == Capacity;
        public bool IsEmpty => Item == null;
        public IItemContainer Item { get; private set; }
        public Type ItemType => Item.Type;
        public int Amount => IsEmpty ? 0 : Item.Amount;
        public int Capacity { get; private set; }

        public ItemSlot()
        {
            if(!IsEmpty)
                return;
        }
        
        public ItemSlot(IItemContainer item)
        {
            if(!IsEmpty)
                return;

            Item = item;
            Capacity = Item.MaxItemsInStack;
        }
        
        public void SetItem(IItemContainer item)
        {
            
        }

        public void Clear()
        {
            if(IsEmpty)
                return;

            Item.Amount = 0;
            Item = null;
        }
    }
}
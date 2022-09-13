using System;
using InventorySystem.Core;
using InventorySystem.Items;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class ItemSlot : ISlotContainer
    {
        [field: SerializeField] public Item Item { get; private set; }
        [field: SerializeField] public int Amount { get; set; }
        [field: SerializeField] public bool IsEquipped { get; set; }

        public bool IsFull => Amount == Capacity;
        public bool IsEmpty => Item == null;
        public Type ItemType => Item.Type;
        public int Capacity { get; private set; }

        public ItemSlot()
        {
            Item = null;
            Amount = 0;
        }
        
        public ItemSlot(Item item, int amount)
        {
            Item = item;
            Amount = amount;
            Capacity = Item.MaxItemsInStack;
        }
        
        public void SetItem(IItemContainer item, int amount)
        {
            if(!IsEmpty)
                return;

            Item = item as Item;
            
            Capacity = Item.MaxItemsInStack;
            Amount = amount;
        }

        public void Clear()
        {
            if(IsEmpty)
                return;

            Amount = 0;
            Item = null;
        }
    }
}
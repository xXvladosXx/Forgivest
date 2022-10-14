using System;
using System.Collections.Generic;
using InventorySystem.Core;
using InventorySystem.Items;
using InventorySystem.Items.Core;
using InventorySystem.Items.ItemTypes.Core;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class ItemSlot : ISlotContainer
    {
        [field: SerializeField] public Item Item { get; private set; }
        [field: SerializeField] public int Amount { get; set; }
        [field: SerializeField] public bool IsEquipped { get; set; }
        
        [field: SerializeField] public List<ItemType> ItemTypes { get; set; }
        
        public bool IsFull => Amount == Capacity;
        public bool IsFullWithMaxStack => Amount == Item.MaxItemsInStack;
        public bool IsEmpty => (Item == null || Amount == 0);
        public Type ItemGetType => Item.Type;
        public int Capacity { get; set; }

        public ItemSlot()
        {
            Item = null;
            Amount = 0;
        }
        
        public ItemSlot(Item item, int amount)
        {
            Item = item;
            Amount = amount;
        }
        
        public void SetItem(IItem item, int amount)
        {
            Item = item as Item;
            Amount = amount;
        }

        public void AddAmount(int amount)
        {
            Amount += amount;
        }

        public bool AllRequirementsChecked(Item item, int amount)
        {
            foreach (var itemType in ItemTypes)
            {
                if (itemType != item.ItemType)
                    return false;
            }

            if (IsFullWithMaxStack)
                return false;
            
            if (amount + Amount > Capacity)
                return false;

            if (amount + Amount > Item.MaxItemsInStack)
                return false;
            
            return true;
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
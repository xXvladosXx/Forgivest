using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool IsFullWithMaxStack
        {
            get
            {
                if (Item != null)
                {
                    return Amount == Item.MaxItemsInStack;
                }

                return false;
            }
        }

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
            if (ItemTypes.Count != 0)
            {
                if (ItemTypes.Any(itemType => itemType != item.ItemType))
                {
                    return false;
                }
            }

            return !IsFullWithMaxStack;
        }

        public void Clear()
        {
            if (IsEmpty)
                return;

            Amount = 0;
            Item = null;
        }
    }
}
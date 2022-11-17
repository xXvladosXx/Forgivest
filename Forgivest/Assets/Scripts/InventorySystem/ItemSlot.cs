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
        [field: SerializeField] public bool Changeable { get; private set; } = true;
        [field: SerializeField] public List<ItemType> ItemTypes { get; private set; }
        [field: SerializeField] public List<ItemType> ProhibitedItemTypes { get; private set; }
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

        public Type ItemGetType => Item == null ? null : Item.Type;

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

        public ItemSlot(Item slotItem, int slotAmount, bool changeable, List<ItemType> slotProhibitedItemTypes, List<ItemType> allowedTypes)
        {
            Item = slotItem;
            Amount = slotAmount;
            Changeable = changeable;
            ProhibitedItemTypes = slotProhibitedItemTypes;
            ItemTypes = allowedTypes;
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
            if (ProhibitedItemTypes.Count != 0)
            {
                if (ProhibitedItemTypes.Any(prohibitedType => prohibitedType == item.ItemType))
                {
                    return false;
                }
            }
            
            if (ItemTypes.Count != 0)
            {
                if (ItemTypes.Any(itemType => itemType != item.ItemType))
                {
                    return false;
                }
            }

            return true;
        }

        public void Clear()
        {
            if (IsEmpty)
                return;

            Amount = 0;
            Item = null;
        }
        
        public string GetItemName()
        {
            return Item == null ? "No name" : Item.ColouredName();
        }

        public string GetDescription()
        {
            return Item == null ? "" : Item.ItemDescription;
        }

        public string GetRequirements()
        {
            return Item == null ? "" : Item.ItemRequirements;
        }
    }
}
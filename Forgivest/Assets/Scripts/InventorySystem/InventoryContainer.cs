using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Core;

namespace InventorySystem
{
    [Serializable]
    public class InventoryContainer : IContainer
    {
        public int Capacity { get; set; }
        public bool IsFull => _slots.All(slot => slot.IsFull);

        private List<ISlotContainer> _slots;

        public event Action<object, IItemContainer, int> OnItemAdded;
        public event Action<object, Type, int> OnItemRemoved;
        
        public InventoryContainer(int capacity)
        {
            Capacity = capacity;
            _slots = new List<ISlotContainer>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                _slots.Add(new ItemSlot());
            }
        }

        public IItemContainer GetItem(Type itemType) => _slots.Find(slot => slot.ItemType == itemType).Item;

        public IItemContainer[] GetAllItems() => (from slot in _slots where !slot.IsEmpty select slot.Item).ToArray();

        public IItemContainer[] GetAllItems(Type itemType) => (from slot in _slots where !slot.IsEmpty 
            && slot.ItemType == itemType select slot.Item).ToArray();

        public IItemContainer[] GetEquippedItems() => (from slot in _slots where !slot.IsEmpty 
            && slot.Item.IsEquipped select slot.Item).ToArray();

        public int GetItemAmount(Type itemType)
        {
            var allItemSlots = _slots.FindAll(slot => !slot.IsEmpty && slot.ItemType == itemType);

            return allItemSlots.Sum(itemSlot => itemSlot.Amount);
        }

        public bool TryToAdd(object sender, IItemContainer item)
        {
            var slotWithSameItemButNotEmpty = _slots.Find(slot => !slot.IsEmpty && slot.ItemType == item.Type && !slot.IsFull);

            if (slotWithSameItemButNotEmpty != null)
            {
                return TryToAddToSlot(sender, slotWithSameItemButNotEmpty, item);
            }
            
            var emptySlot = _slots.Find(slot => slot.IsEmpty);
            if (emptySlot != null)
            {
                return TryToAddToSlot(sender, emptySlot, item);
            }

            return false;
        }

        private bool TryToAddToSlot(object sender, ISlotContainer slot, IItemContainer item)
        {
            var fits = slot.Amount + item.Amount <= item.MaxItemsInStack;
            var amountToAdd = fits ? item.Amount : item.MaxItemsInStack - slot.Amount;
            var amountLeft = item.Amount -= amountToAdd;

            if (slot.IsEmpty)
            {
                //slot.SetItem(clonedItem);
            }
            else
            {
                slot.Item.Amount += amountToAdd;
            }

            OnItemAdded?.Invoke(sender, item, amountToAdd);
            
            if (amountLeft <= 0)
            {
                return true;
            }

            item.Amount = amountLeft;
            return TryToAdd(sender, item);
        }

        public void Remove(object sender, Type itemType, int amount = 1)
        {
            var slotsWithItem = GetAllSlots(itemType);
            if(slotsWithItem.Length == 0)
                return;

            var amountToRemove = amount;
            var count = slotsWithItem.Length;

            for (int i = count - 1;i >= 0; i--)
            {
                var slot = slotsWithItem[i];
                if (slot.Amount >= amountToRemove)
                {
                    slot.Item.Amount -= amountToRemove;
                    
                    if(slot.Amount <= 0)
                        slot.Clear();
                    
                    OnItemRemoved?.Invoke(sender, itemType, amountToRemove);
                    
                    break;
                }

                var amountRemoved = slot.Amount;
                amountToRemove -= slot.Amount;
                slot.Clear();
                
                OnItemRemoved?.Invoke(sender, itemType, amountRemoved);
            }
        }


        public bool HasItem(Type type)
        {
            var containerItem = GetItem(type);
            return containerItem != null;
        }
        
        private ISlotContainer[] GetAllSlots(Type itemType) => _slots.FindAll(slot => !slot.IsEmpty && slot.ItemType == itemType).ToArray();
        private ISlotContainer[] GetAllSlots() => _slots.ToArray();
    }
}
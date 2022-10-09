using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Core;
using InventorySystem.Items;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class ItemContainer : IContainer
    {
        [field: SerializeField] public List<ItemSlot> Slots { get; private set; } = new List<ItemSlot>();
        public int Capacity { get; set; }
        public bool IsFull => Slots.All(slot => slot.IsFull);

        public event Action<object, IItemContainer, int> OnItemAdded;
        public event Action<object, Type, int> OnItemRemoved;

        
        public void Init(int capacity)
        {
            Capacity = capacity;
            if (Slots.Count < capacity)
            {
                for (int i = Slots.Count; i < capacity; i++)
                {
                    Slots.Add(new ItemSlot());
                }
            }
            
            for (int i = 0; i < capacity; i++)
            {
                if (Slots[i] == null)
                {
                    Slots.Add(new ItemSlot());
                }
            }
        }
       
        public IItemContainer GetItem(Type itemType) => Slots.Find(slot => slot.ItemType == itemType).Item;

        public IItemContainer[] GetAllItems() =>
            (Slots.Where(slot => !slot.IsEmpty).Select(slot => slot.Item)).ToArray();

        public IItemContainer[] GetAllItems(Type itemType) => (Slots
            .Where(slot => !slot.IsEmpty && slot.ItemType == itemType)
            .Select(slot => slot.Item)).ToArray();

        public IItemContainer[] GetEquippedItems() => (Slots.Where(slot => !slot.IsEmpty && slot.IsEquipped)
            .Select(slot => slot.Item)).ToArray();

        public int GetItemAmount(Type itemType)
        {
            var allItemSlots = Slots.FindAll(slot => !slot.IsEmpty && slot.ItemType == itemType);

            return allItemSlots.Sum(itemSlot => itemSlot.Amount);
        }

        public bool TryToAdd(object sender, IItemContainer item, int amount)
        {
            var slotWithSameItemButNotEmpty = Slots
                .Find(slot => !slot.IsEmpty && slot.Item.ItemID.Id == item.ItemID.Id && !slot.IsFull);
            
            if (slotWithSameItemButNotEmpty is {IsFull: false})
            {
                return TryToAddToSlot(sender, slotWithSameItemButNotEmpty, item, amount);
            }

            var emptySlot = Slots.Find(slot => slot.IsEmpty);
            if (emptySlot != null)
            {
                return TryToAddToSlot(sender, emptySlot, item, amount);
            }

            return false;
        }

        private bool TryToAddToSlot(object sender, ISlotContainer slot, IItemContainer item, int amount)
        {
            var fits = slot.Amount + amount <= item.MaxItemsInStack;
            var amountToAdd = fits ? amount : item.MaxItemsInStack - slot.Amount;
            var amountLeft = amount -= amountToAdd;

            if (slot.IsEmpty)
            {
                slot.SetItem(item, amountToAdd);
            }
            else
            {
                slot.Amount += amountToAdd;
            }

            OnItemAdded?.Invoke(sender, item, amountToAdd);

            if (amountLeft <= 0)
            {
                return true;
            }

            return TryToAdd(sender, item, amountLeft);
        }

        public void Remove(object sender, Type itemType, int amount = 1)
        {
            var slotsWithItem = GetAllSlots(itemType);
            if (slotsWithItem.Length == 0)
                return;

            var amountToRemove = amount;
            var count = slotsWithItem.Length;

            for (int i = count - 1; i >= 0; i--)
            {
                var slot = slotsWithItem[i];
                if (slot.Amount >= amountToRemove)
                {
                    slot.Amount -= amountToRemove;

                    if (slot.Amount <= 0)
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

        private ISlotContainer[] GetAllSlots(Type itemType) =>
            Slots.FindAll(slot => !slot.IsEmpty && slot.ItemType == itemType).ToArray();

        private ISlotContainer[] GetAllSlots() => Slots.ToArray();

        
    }
}
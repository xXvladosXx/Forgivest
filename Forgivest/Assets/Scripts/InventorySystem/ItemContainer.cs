using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Core;
using InventorySystem.Items;
using InventorySystem.Items.Core;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class ItemContainer : IContainer
    {
        [field: SerializeField] public List<ItemSlot> Slots { get; private set; } = new List<ItemSlot>();
        public int Capacity { get; set; }
        public bool IsFull => Slots.All(slot => slot.IsFull);

        public event Action<object, IItem, int, ItemContainer> OnItemAdded;
        public event Action<object, IItem, int, ItemContainer> OnItemRemoved;
        public event Action<int, int, ItemContainer> OnItemSwapped;


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

        public void InitSlots()
        {
            foreach (var itemSlot in Slots)
            {
                itemSlot.Capacity = 64;
            }
        }

        public IItem GetItem(Type itemType) => Slots.Find(slot => slot.ItemGetType == itemType).Item;

        public Item[] GetAllItems() =>
            (Slots.Where(slot => !slot.IsEmpty).Select(slot => slot.Item)).ToArray();

        public Item[] GetAllItems(Type itemType) => (Slots
            .Where(slot => !slot.IsEmpty && slot.ItemGetType == itemType)
            .Select(slot => slot.Item)).ToArray();

        public Item[] GetEquippedItems() => (Slots.Where(slot => !slot.IsEmpty && slot.IsEquipped)
            .Select(slot => slot.Item)).ToArray();

        public int GetItemAmount(Type itemType)
        {
            var allItemSlots = Slots.FindAll(slot => !slot.IsEmpty && slot.ItemGetType == itemType);

            return allItemSlots.Sum(itemSlot => itemSlot.Amount);
        }

        public bool TryToAdd(object sender, IItem item, int amount, ItemContainer destinationContainer = null)
        {
            var slotWithSameItemButNotEmpty = Slots
                .Find(slot => !slot.IsEmpty && slot.Item.ItemID.Id == item.ItemID.Id && !slot.IsFull);

            if (slotWithSameItemButNotEmpty is { IsFullWithMaxStack: false })
            {
                return TryToAddToSlot(sender, slotWithSameItemButNotEmpty, item, amount, destinationContainer);
            }

            var emptySlot = Slots.Find(slot => slot.IsEmpty);
            if (emptySlot != null)
            {
                return TryToAddToSlot(sender, emptySlot, item, amount, destinationContainer);
            }

            return false;
        }

        private bool TryToAddToSlot(object sender, ISlotContainer slot, IItem item, int amount,
            ItemContainer destinationContainer)
        {
            if (destinationContainer != null)
            {
                if (slot.Item != (Item)item)
                {
                    OnItemAdded?.Invoke(sender, item, amount, this);

                    slot.SetItem(item, amount);
                }

                return true;
            }

            if (!slot.AllRequirementsChecked((Item)item, amount))
            {
                return false;
            }
            
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
                return false;
            }

            OnItemAdded?.Invoke(sender, item, amountToAdd, this);

            if (amountLeft <= 0)
            {
                return true;
            }

            return TryToAdd(sender, item, amountLeft);
        }

        public bool Remove(object sender, int index, bool max = false, int amount = 1)
        {
            var slot = Slots[index];
            if (slot.IsEmpty)
                return false;

            if (max)
            {
                OnItemRemoved?.Invoke(sender, slot.Item, slot.Amount, this);

                slot.SetItem(null, 0);
                
                return true;
            }

            var amountToRemove = amount;
            if (slot.Amount < amountToRemove)
            {
                return false;
            }

            slot.Amount -= amountToRemove;

            if (slot.Amount == 0)
            {
                OnItemRemoved?.Invoke(sender, slot.Item, amountToRemove, this);

                slot.SetItem(null, 0);
            }

            return true;
        }


        public bool HasItem(Type type)
        {
            var containerItem = GetItem(type);
            return containerItem != null;
        }


        public void DropItemIntoContainer(int source, int destination, ItemContainer destinationContainer = null)
        {
            var sourceSlot = Slots[source];
            var destinationSlot = destinationContainer?.Slots[destination] ?? Slots[destination];

            if (sourceSlot == null || destinationSlot == null
                                   || sourceSlot.Item == null || destinationSlot.Item == null)
            {
                AttemptSimpleTransfer(source, destination, destinationContainer);
                OnItemSwapped?.Invoke(source, destination, this);
                destinationContainer?.OnItemSwapped?.Invoke(source, destination, destinationContainer);

                return;
            }

            if (sourceSlot.Item == destinationSlot.Item)
            {
                AttemptStackTransfer(source, destination, destinationContainer);
                OnItemSwapped?.Invoke(source, destination, this);
                destinationContainer?.OnItemSwapped?.Invoke(source, destination, destinationContainer);

                return;
            }

            AttemptSwap(source, destination, destinationContainer);
            OnItemSwapped?.Invoke(source, destination, this);
            destinationContainer?.OnItemSwapped?.Invoke(source, destination, destinationContainer);
        }

        private bool AttemptSimpleTransfer(int source, int destination, ItemContainer destinationContainer)
        {
            var draggingItem = Slots[source].Item;
            var draggingNumber = Slots[source].Amount;
            var destinationSlot = destinationContainer?.Slots[destination] ?? Slots[destination];

            var acceptable = destinationSlot.Capacity;
            var toTransfer = Mathf.Min(64, draggingNumber);

            if (destinationSlot.AllRequirementsChecked(draggingItem, draggingNumber))
            {
                if (toTransfer > 0)
                {
                    Remove(this, source, false, toTransfer);

                    destinationSlot.SetItem(draggingItem, toTransfer);

                    destinationContainer?.OnItemAdded?.Invoke(this, draggingItem, draggingNumber, destinationContainer);
                    return false;
                }
            }

            return true;
        }

        private bool AttemptStackTransfer(int source, int destination, ItemContainer destinationContainer)
        {
            var draggingItem = Slots[source].Item;
            var draggingNumber = Slots[source].Amount;
            var destinationSlot = destinationContainer?.Slots[destination] ?? Slots[destination];

            var sourceTakeBackNumber = CalculateTakeBack(Slots[source], destinationSlot);
            var amountToAddToSlot = destinationSlot.Item.MaxItemsInStack - destinationSlot.Amount;

            if (sourceTakeBackNumber > 0)
            {
                Remove(this, source, false, amountToAddToSlot);

                destinationSlot.AddAmount(amountToAddToSlot);

                return true;
            }

            if (destinationSlot.AllRequirementsChecked(draggingItem, draggingNumber))
            {
                Remove(this, source, false, draggingNumber);
                destinationSlot.AddAmount(draggingNumber);

                return true;
            }

            return false;
        }


        public void AttemptSwap(int source, int destination, ItemContainer destinationContainer = null)
        {
            var sourceSlot = Slots[source];
            var destinationSlot = destinationContainer?.Slots[destination] ?? Slots[destination];

            var sourceItem = sourceSlot.Item;
            var destinationItem = destinationSlot.Item;

            int removedSourceNumber = sourceSlot.Amount;
            int removedDestinationNumber = destinationSlot.Amount;

            if (!sourceSlot.AllRequirementsChecked(destinationItem, removedDestinationNumber)) return;
            if (!destinationSlot.AllRequirementsChecked(sourceItem, removedDestinationNumber)) return;

            Remove(this, source, false, sourceSlot.Amount);
            Remove(this, destination, false, destinationSlot.Amount);

            var sourceTakeBackNumber = CalculateTakeBack(sourceSlot, sourceSlot.Amount);
            var destinationTakeBackNumber = CalculateTakeBack(destinationSlot, destinationSlot.Amount);

            if (sourceTakeBackNumber > 0)
            {
                TryToAddToSlot(this, sourceSlot, sourceItem, removedSourceNumber, destinationContainer);
                removedSourceNumber -= sourceTakeBackNumber;
            }

            if (destinationTakeBackNumber > 0)
            {
                TryToAddToSlot(this, destinationSlot, destinationItem, removedDestinationNumber, destinationContainer);
                removedDestinationNumber -= destinationTakeBackNumber;
            }

            if (64 < removedDestinationNumber || 64 < removedSourceNumber)
            {
                TryToAddToSlot(this, destinationSlot, destinationItem, removedDestinationNumber, destinationContainer);
                TryToAddToSlot(this, sourceSlot, sourceItem, removedSourceNumber, destinationContainer);

                return;
            }

            if (removedDestinationNumber > 0)
            {
                TryToAddToSlot(this, destinationSlot, sourceItem, removedSourceNumber, destinationContainer);
            }

            if (removedSourceNumber > 0)
            {
                TryToAddToSlot(this, sourceSlot, destinationItem, removedDestinationNumber, destinationContainer);
            }
        }


        private int CalculateTakeBack(ItemSlot sourceItem, int removedNumber)
        {
            var takeBackNumber = 0;
            var destinationMaxAcceptable = sourceItem.Capacity;

            if (destinationMaxAcceptable < removedNumber)
            {
                takeBackNumber = removedNumber - destinationMaxAcceptable;

                var sourceTakeBackAcceptable = sourceItem.Capacity;

                if (sourceTakeBackAcceptable < takeBackNumber)
                {
                    return 0;
                }
            }

            return takeBackNumber;
        }


        private int CalculateTakeBack(ItemSlot sourceSlot, ItemSlot destinationSlot)
        {
            var takeBackValue = 0;
            var sourceSlotAmount = sourceSlot.Amount;
            var destinationSlotAmount = destinationSlot.Amount;

            var maxInStack = destinationSlot.Item.MaxItemsInStack;
            var finalAmount = destinationSlotAmount + sourceSlotAmount;

            if (finalAmount > maxInStack)
            {
                takeBackValue = finalAmount - maxInStack;
            }

            return takeBackValue;
        }

        private ItemSlot[] GetAllSlots(Type itemType) =>
            Slots.FindAll(slot => !slot.IsEmpty && slot.ItemGetType == itemType).ToArray();

        private ItemSlot[] GetAllSlots() => Slots.ToArray();
    }
}
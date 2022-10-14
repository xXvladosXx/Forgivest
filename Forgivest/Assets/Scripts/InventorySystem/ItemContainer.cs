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

        public event Action<object, IItem, int> OnItemAdded;
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

        public bool TryToAdd(object sender, IItem item, int amount)
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

        private bool TryToAddToSlot(object sender, ISlotContainer slot, IItem item, int amount)
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
        
        public bool Remove(object sender, int index, int amount = 1)
        {
            var slot = Slots[index];
            if (slot.IsEmpty)
                return false;
            
            var amountToRemove = amount;
            if (slot.Amount < amountToRemove)
            {
                return false;
            }
            
            slot.Amount -= amountToRemove;

            if (slot.Amount == 0)
            {
                slot.SetItem(null, 0);
            }
            
            OnItemRemoved?.Invoke(sender, slot.ItemGetType, amountToRemove);
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
                || sourceSlot.Item == null|| destinationSlot.Item == null)
            {
                AttemptSimpleTransfer(source, destination, destinationContainer);
                
                //OnItemSlotChanged?.Invoke(destination.Index );
                //print($"item swapped {destination.Index }");
                return;
            }

            if(sourceSlot.Item == destinationSlot.Item)
            {
                AttemptStackTransfer(source, destination, destinationContainer);
                
                //OnItemSlotChanged?.Invoke(destination.Index );
                //print($"item stacked {destination.Index }");
                return;
            }
            
            
            AttemptSwap(source, destination, destinationContainer);
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
                Remove(this, source, amountToAddToSlot);

                destinationSlot.AddAmount(amountToAddToSlot);
                
                return true;
            }
            
            if (destinationSlot.AllRequirementsChecked(draggingItem, draggingNumber))
            {
                Remove(this, source, draggingNumber);
                destinationSlot.AddAmount(draggingNumber);
                
//              OnItemSwapped?.Invoke(_source.SourceIndex, destination.Index );
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

            Remove(this, source, sourceSlot.Amount);
            Remove(this, destination, destinationSlot.Amount);

            var sourceTakeBackNumber = CalculateTakeBack(sourceSlot, sourceSlot.Amount);
            var destinationTakeBackNumber = CalculateTakeBack(destinationSlot, destinationSlot.Amount);

            if (sourceTakeBackNumber > 0)
            {
                TryToAddToSlot(this, sourceSlot, sourceSlot.Item, sourceSlot.Amount);
                removedSourceNumber -= sourceTakeBackNumber;
            }
            if (destinationTakeBackNumber > 0)
            {
                TryToAddToSlot(this, destinationSlot, destinationSlot.Item, destinationSlot.Amount);
                removedDestinationNumber -= destinationTakeBackNumber;
            }

            if (sourceSlot.Capacity < removedDestinationNumber || destinationSlot.Capacity < removedSourceNumber)
            {
                TryToAddToSlot(this, destinationSlot, destinationSlot.Item, removedDestinationNumber);
                TryToAddToSlot(this, sourceSlot, sourceSlot.Item, removedSourceNumber);

                //OnItemSwapped?.Invoke(source.Index , destination.Index );
                //print($"item swapped {source.Index }, {destination.Index }");
                return;
            }

            if (removedDestinationNumber > 0)
            {
                TryToAddToSlot(this, destinationSlot, sourceItem, removedSourceNumber);
            }
            if (removedSourceNumber > 0)
            {
                TryToAddToSlot(this, sourceSlot, destinationItem, removedDestinationNumber);
            }
            
            //OnItemSwapped?.Invoke(source.Index, destination.Index );
            //print($"item swapped {source.SourceIndex }, {destination.Index }");
        }
        
        private bool AttemptSimpleTransfer(int source, int destination, ItemContainer destinationContainer)
        {
            var draggingItem = Slots[source].Item;
            var draggingNumber = Slots[source].Amount;
            var destinationSlot = destinationContainer?.Slots[destination] ?? Slots[destination];
            
            var acceptable = destinationSlot.Capacity;
            var toTransfer = Mathf.Min(acceptable, draggingNumber);

            if (toTransfer > 0)
            {
                Remove(this, source, toTransfer);
                
                destinationSlot.SetItem(draggingItem, toTransfer);
                
//                OnItemSwapped?.Invoke(_source.SourceIndex, destination.Index );

                return false;
            }

            return true;
        }

        private int CalculateTakeBack(ItemSlot sourceItem,int removedNumber)
        {
            var takeBackNumber = 0;
            var destinationMaxAcceptable = sourceItem.Capacity;

            if (destinationMaxAcceptable < removedNumber)
            {
                takeBackNumber = removedNumber - destinationMaxAcceptable;

                var sourceTakeBackAcceptable = sourceItem.Capacity;

                // Abort and reset
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
                
            if(finalAmount > maxInStack)
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
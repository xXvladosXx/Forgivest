using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem;
using InventorySystem.Core;
using InventorySystem.Interaction;
using UI.Inventory;
using UI.Inventory.Core;
using UnityEngine;
using Zenject;

namespace Controllers.Player
{
    public class InventoryController : IInitializable, ITickable, IDisposable
    {
        private readonly InventoryPanel _inventoryPanel;
        private readonly ObjectPicker _objectPicker;

        private Dictionary<ItemContainer, ItemContainerUI> _itemHolders =
            new Dictionary<ItemContainer, ItemContainerUI>();

        public InventoryController(InventoryPanel inventoryPanel, ObjectPicker objectPicker)
        {
            _inventoryPanel = inventoryPanel;
            _objectPicker = objectPicker;
        }
        
        public void Initialize()
        {
            _inventoryPanel.InitializeInventory(_objectPicker.Inventory.Capacity);
            _inventoryPanel.InitializeEquipment(_objectPicker.Equipment.Capacity);
            _inventoryPanel.InitializeHotbar(_objectPicker.Hotbar.Capacity);
            
            _itemHolders.Add(_objectPicker.Inventory.ItemContainer, _inventoryPanel.InventoryItemContainerUI);
            _itemHolders.Add(_objectPicker.Equipment.ItemContainer, _inventoryPanel.EquipmentItemContainerUI);
            _itemHolders.Add(_objectPicker.Hotbar.ItemContainer, _inventoryPanel.HotbarContainerUI);

            foreach (var itemContainer in _itemHolders.Keys)
            {
                RefreshSlotsData(itemContainer);
                itemContainer.OnItemAdded += OnItemAdded;
            }

            foreach (var itemContainer in _itemHolders.Values)
            {
                itemContainer.OnTryToSwapSlots += TryToSwapSlotsInInventory;
            }
        }


        public void Tick()
        {
            
        }

        public void Dispose()
        {
            foreach (var itemContainer in _itemHolders.Keys)
            {
                itemContainer.OnItemAdded -= OnItemAdded;
            }

            foreach (var itemContainer in _itemHolders.Values)
            {
                itemContainer.OnTryToSwapSlots -= TryToSwapSlotsInInventory;
            }
        }
        

        private void OnSwapCompleted(int source, int destination, ItemContainer itemContainer)
        {
            itemContainer.OnItemSwapped -= OnSwapCompleted;
            RefreshSlotsData(itemContainer);
        }
        
        private void TryToSwapSlotsInInventory(int source, int destination, Sprite sprite, int amount, IInventoryHolder sourceInventoryHolder)
        {
            if (sourceInventoryHolder != _inventoryPanel.LastRaycastedInventoryHolder)
            {
                var destinationContainer = _itemHolders.FirstOrDefault(
                    x => x.Value == (ItemContainerUI) _inventoryPanel.LastRaycastedInventoryHolder)
                    .Key;

                foreach (var itemContainer in _itemHolders)
                {
                    if (itemContainer.Value == (ItemContainerUI) _inventoryPanel.LastRaycastedInventoryHolder) continue;
                    if (itemContainer.Value != (ItemContainerUI) sourceInventoryHolder) continue;
                    
                    itemContainer.Key.OnItemSwapped += OnSwapCompleted;
                    itemContainer.Key.DropItemIntoContainer(source,destination, destinationContainer);
                    itemContainer.Key.OnItemSwapped -= OnSwapCompleted;
                    RefreshSlotsData(destinationContainer);
                    return;
                }
            }
            
            foreach (var itemContainer in _itemHolders)
            {
                if (itemContainer.Value != (ItemContainerUI) sourceInventoryHolder) continue;

                itemContainer.Key.OnItemSwapped += OnSwapCompleted;
                itemContainer.Key.DropItemIntoContainer(source,destination);
                itemContainer.Key.OnItemSwapped -= OnSwapCompleted;
                        
                return;
            }
        }
        
        private void RefreshSlotsData(ItemContainer itemContainer)
        {
            int index = 0;
            foreach (var slot in itemContainer.Slots)
            {
                if (slot.Item == null)
                {
                    _inventoryPanel.CreateSlotsWithItemInInventory(null, 0, index, _itemHolders[itemContainer]);
                    index++;
                    continue;
                }

                _inventoryPanel.CreateSlotsWithItemInInventory(slot.Item.Sprite, slot.Amount, index, _itemHolders[itemContainer]);
                index++;
            }
        }
        
        private void OnItemAdded(object sender, IItem item, int amount, ItemContainer itemContainer)
        {
            RefreshSlotsData(itemContainer);
        }
    }
}
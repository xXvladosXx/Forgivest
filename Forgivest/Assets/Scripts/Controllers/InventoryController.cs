using System;
using InventorySystem;
using UI.Inventory;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class InventoryController : IInitializable, ITickable, IDisposable
    {
        private readonly InventoryPanel _inventoryPanel;
        private readonly Inventory _inventory;

        public InventoryController(InventoryPanel inventoryPanel, Inventory inventory)
        {
            _inventoryPanel = inventoryPanel;
            _inventory = inventory;
        }
        
        public void Initialize()
        {
            _inventoryPanel.InitializeInventory(_inventory.Capacity);
            InitializeSlots();

            _inventoryPanel.InventoryItemContainerUI.OnSlotsSwapped += ChangeSlotsInInventory;
        }

        public void Tick()
        {
            
        }

        public void Dispose()
        {
            _inventoryPanel.InventoryItemContainerUI.OnSlotsSwapped -= ChangeSlotsInInventory;
        }
        
        private void InitializeSlots()
        {
            int index = 0;
            foreach (var slot in _inventory.ItemContainer.Slots)
            {
                if (slot.Item == null)
                {
                    _inventoryPanel.CreateSlotsWithItem(null, 0, index);
                    index++;
                    continue;
                }

                _inventoryPanel.CreateSlotsWithItem(slot.Item.Sprite, slot.Amount, index);
                index++;
            }
        }

        private void ChangeSlotsInInventory(int source, int destination)
        {
            //TO DO swap
            //_inventory.ItemContainer.(source, destination);
        }
    }
}
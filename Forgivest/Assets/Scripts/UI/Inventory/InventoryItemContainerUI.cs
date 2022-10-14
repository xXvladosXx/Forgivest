using System;
using System.Collections.Generic;
using GameDevTV.UI.Inventories;
using UI.Core;
using UI.Inventory.Core;
using UI.Inventory.Dragging;
using UnityEngine;

namespace UI.Inventory
{
    public class InventoryItemContainerUI : UIElement
    {
        [SerializeField] private int _capacity;
        [SerializeField] private InventorySlotUI _inventorySlotUI;
        [SerializeField] private Transform _inventorySlotParent;

        private List<InventorySlotUI> _inventorySlots = new List<InventorySlotUI>();

        public event Action<int, int> OnSlotsSwapped; 
        public event Action<int, IDragDestination> OnSlotsDragEnded; 
        public void InitializeSlots(int inventoryCapacity)
        {
            ClearSlots();
            CreateSlots(inventoryCapacity);
        }

        private void CreateSlots(int inventoryCapacity)
        {
            for (int i = 0; i < inventoryCapacity; i++)
            {
                var inventorySlotUI = Instantiate(_inventorySlotUI, _inventorySlotParent);
                _inventorySlots.Add(inventorySlotUI);
                //inventorySlotUI.InventoryDragItem.OnItemSwapped += OnItemSwapped;
                //inventorySlotUI.InventoryDragItem.OnDragEnded += OnDragEnded;
                
                _capacity++;
            }
        }

        private void OnDragEnded(int index, IDragDestination container)
        {
            //OnSlotsSwapped?.Invoke(index, container);
        }

        private void OnItemSwapped(int source, int destination)
        {
            print("Source " + source + " Destination " + destination);
            OnSlotsSwapped?.Invoke(source, destination);
        }

        private void Update()
        {
            foreach (var inventorySlot in _inventorySlots)
            {
                print(inventorySlot.GetIcon() + " " + inventorySlot.Index + "\n");
            }
        }

        private void ClearSlots()
        {
            foreach (Transform child in _inventorySlotParent)
            {
                Destroy(child);
            }
        }

        public void RefreshSlotData(Sprite itemSprite, int itemAmount, int slotIndex)
        {
            if(_inventorySlots[slotIndex] != null)
            {
                _inventorySlots[slotIndex].SetItemData(itemSprite, itemAmount, slotIndex);
            }
        }

        private void OnDestroy()
        {
            foreach (var inventorySlot in _inventorySlots)
            {
                //inventorySlot.InventoryDragItem.OnItemSwapped -= OnItemSwapped;
            }
        }
    }
}
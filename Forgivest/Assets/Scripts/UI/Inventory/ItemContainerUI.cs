using System;
using System.Collections.Generic;
using UI.Core;
using UI.Inventory.Core;
using UI.Inventory.Slot;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Inventory
{
    public abstract class ItemContainerUI : UIElement, IInventoryHolder
    {
        [SerializeField] private InventorySlotUI _inventorySlotUI;
        [SerializeField] private Transform _inventorySlotParent;

        protected readonly List<InventorySlotUI> InventorySlotUis = new List<InventorySlotUI>();

        public event Action<int, int, Sprite, int, IInventoryHolder> OnTryToSwapSlots; 
        public event Action<int, Sprite, int, IInventoryHolder> OnTryToDropSlot; 
        public event Action<IInventoryHolder> OnInventoryHolderChanged; 
        public void InitializeSlots(int inventoryCapacity)
        {
            ClearSlots();
            CreateSlots(inventoryCapacity);
        }

        protected virtual void CreateSlots(int inventoryCapacity)
        {
            for (int i = 0; i < inventoryCapacity; i++)
            {
                var inventorySlotUI = Instantiate(_inventorySlotUI, _inventorySlotParent);
                InventorySlotUis.Add(inventorySlotUI);
                inventorySlotUI.OnItemTryToSwap += OnItemTryToSwap;
                inventorySlotUI.OnItemTryToDrop += OnItemTryToDrop;
            }
        }

        protected void OnItemTryToSwap(int source, int destination, Sprite sprite, int amount, IInventoryHolder inventoryHolder)
        {
            print("Source " + source + " Destination " + destination);
            OnTryToSwapSlots?.Invoke(source, destination, sprite, amount, inventoryHolder);
        }

        protected void OnItemTryToDrop(int source, Sprite sprite, int amount, IInventoryHolder inventoryHolder)
        {
            OnTryToDropSlot?.Invoke(source, sprite, amount, inventoryHolder);
        }

        protected virtual void ClearSlots()
        {
            foreach (Transform child in _inventorySlotParent)
            {
                Destroy(child);
            }
        }

        public void RefreshSlotData(Sprite itemSprite, int itemAmount, int slotIndex, string description, string itemName, string requirements)
        {
            if(InventorySlotUis[slotIndex] != null)
            {
                InventorySlotUis[slotIndex].SetSlotData(itemSprite, itemAmount, slotIndex,
                    description, itemName, requirements);
            }
        }

        private void OnDestroy()
        {
            foreach (var inventorySlot in InventorySlotUis)
            {
                 inventorySlot.OnItemTryToSwap -= OnItemTryToSwap;
                 inventorySlot.OnItemTryToDrop -= OnItemTryToDrop;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnInventoryHolderChanged?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnInventoryHolderChanged?.Invoke(null);
        }
    }
}
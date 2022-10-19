using System.Collections.Generic;
using UI.Core;
using UnityEngine;

namespace UI.Inventory.Core
{
    public class InventoryPanel : Panel
    {
        [SerializeField] private List<UIElement> _uiElements;
        [SerializeField] private InventoryItemContainerUI _inventoryItemContainerUI;
        [SerializeField] private EquipmentItemContainerUI _equipmentItemContainerUI;
        [SerializeField] private HotbarContainerUI _hotbarContainerUI;
        
        public IInventoryHolder LastRaycastedInventoryHolder { get; private set; } 

        public InventoryItemContainerUI InventoryItemContainerUI => _inventoryItemContainerUI;
        public EquipmentItemContainerUI EquipmentItemContainerUI => _equipmentItemContainerUI;
        public HotbarContainerUI HotbarContainerUI => _hotbarContainerUI;
        private void OnEnable()
        {
            _inventoryItemContainerUI.OnInventoryHolderChanged += ChangeHolder;
            _equipmentItemContainerUI.OnInventoryHolderChanged += ChangeHolder;
            _hotbarContainerUI.OnInventoryHolderChanged += ChangeHolder;
        }

        private void OnDisable()
        {
            _inventoryItemContainerUI.OnInventoryHolderChanged -= ChangeHolder;
            _equipmentItemContainerUI.OnInventoryHolderChanged -= ChangeHolder;
            _hotbarContainerUI.OnInventoryHolderChanged -= ChangeHolder;
        }

        public override void Show()
        {
            base.Show();
            foreach (var uiElement in _uiElements)
            {
                uiElement.Show();
            }
        }
        
        public override void Hide()
        {
            base.Hide();
            foreach (var uiElement in _uiElements)
            {
                uiElement.Hide();
            }
        }

        public void InitializeInventory(int inventoryCapacity)
        {
             _inventoryItemContainerUI.InitializeSlots(inventoryCapacity);
        }
        
        public void InitializeEquipment(int equipmentCapacity)
        {
            _equipmentItemContainerUI.InitializeSlots(equipmentCapacity);
        }

        public void InitializeHotbar(int hotbarCapacity)
        {
            _hotbarContainerUI.InitializeSlots(hotbarCapacity);
        }
        
        public void CreateSlotsWithItemInInventory(Sprite sprite, int amount, int index, ItemContainerUI itemContainerUI, string description)
        {
            itemContainerUI.RefreshSlotData(sprite, amount, index, description);
        }
        
        private void ChangeHolder(IInventoryHolder inventoryHolder)
        {
            LastRaycastedInventoryHolder = inventoryHolder;
        }

       
    }
}
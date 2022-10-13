using System.Collections.Generic;
using UI.Core;
using UnityEngine;

namespace UI.Inventory
{
    public class InventoryPanel : Panel
    {
        [SerializeField] private List<UIElement> _uiElements;
        [SerializeField] private InventoryItemContainerUI _inventoryItemContainerUI;
        
        public InventoryItemContainerUI InventoryItemContainerUI => _inventoryItemContainerUI;
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

        public void CreateSlotsWithItem(Sprite sprite, int amount, int index)
        {
            _inventoryItemContainerUI.RefreshSlotData(sprite, amount, index);
        }
    }
}
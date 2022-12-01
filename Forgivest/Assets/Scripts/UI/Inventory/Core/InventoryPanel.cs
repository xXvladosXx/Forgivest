using System.Collections.Generic;
using UI.Core;
using UI.Stats;
using UnityEngine;

namespace UI.Inventory.Core
{
    public class InventoryPanel : UI.Core.InventoryPanel
    {
        [field: SerializeField] public StatsView StatsView { get; private set; }

        [SerializeField] private List<UIElement> _uiElements;
        [SerializeField] private DynamicItemContainerUI _dynamicItemContainerUI;
        [SerializeField] private StaticItemContainerUI _staticItemContainerUI;

        public DynamicItemContainerUI DynamicItemContainerUI => _dynamicItemContainerUI;
        public StaticItemContainerUI StaticItemContainerUI => _staticItemContainerUI;
        private void OnEnable()
        {
            _dynamicItemContainerUI.OnInventoryHolderChanged += ChangeHolder;
            _staticItemContainerUI.OnInventoryHolderChanged += ChangeHolder;
        }

        private void OnDisable()
        {
            _dynamicItemContainerUI.OnInventoryHolderChanged -= ChangeHolder;
            _staticItemContainerUI.OnInventoryHolderChanged -= ChangeHolder;
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
             _dynamicItemContainerUI.InitializeSlots(inventoryCapacity);
        }
        
        public void InitializeEquipment(int equipmentCapacity)
        {
            _staticItemContainerUI.InitializeSlots(equipmentCapacity);
        }
    }
}
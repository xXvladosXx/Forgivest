using UI.Core;
using UI.Inventory;
using UnityEngine;

namespace UI.HUD
{
    public class StaticInventoryPanel : InventoryPanel
    {
        [field: SerializeField] public StaticItemContainerUI HotbarItemContainerUI { get; private set; }
        [field: SerializeField] public WarningUI WarningUI { get; private set; }
        private void OnEnable()
        {
            HotbarItemContainerUI.OnInventoryHolderChanged += ChangeHolder;
        }

        private void OnDisable()
        {
            HotbarItemContainerUI.OnInventoryHolderChanged -= ChangeHolder;
        }

        public void InitializeHotbarInventory(int hotbarCapacity)
        {
            HotbarItemContainerUI.InitializeSlots(hotbarCapacity);
        }
    }
}
using UI.Core;
using UI.Inventory;
using UnityEngine;

namespace UI.HUD
{
    public class StaticPanel : Panel
    {
        [field: SerializeField] public StaticItemContainerUI HotbarItemContainerUI { get; private set; }
        
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
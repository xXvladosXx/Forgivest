using UI.Data;
using UI.Inventory;
using UI.Inventory.Core;
using UnityEngine;

namespace UI.Core
{
    public class Panel : UIElement
    {
        [field: SerializeField] public UIReusableData UIReusalbeData { get; private set; }
        
        public void CreateSlotsWithItemInInventory(Sprite sprite, int amount, int index, ItemContainerUI itemContainerUI, string description)
        {
            itemContainerUI.RefreshSlotData(sprite, amount, index, description);
        }
        
        protected void ChangeHolder(IInventoryHolder inventoryHolder)
        {
            UIReusalbeData.LastRaycastedInventoryHolder = inventoryHolder;
        }
    }
}
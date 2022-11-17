using UI.Inventory.Slot;
using UnityEngine;

namespace UI.Inventory.Tooltips
{
   
    [RequireComponent(typeof(InventorySlotUI))]
    public class ItemTooltipSpawner : TooltipSpawner
    {
        public override bool CanCreateTooltip()
        {
            var item = GetComponent<InventorySlotUI>().GetIcon();
            return item;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
            var itemTooltip = tooltip.GetComponent<ItemTooltip>();
            var item = GetComponent<InventorySlotUI>();
            if (!itemTooltip) return;

            itemTooltip.Setup(item.ItemDescription, item.Name, item.Requirements);
        }
    }
}
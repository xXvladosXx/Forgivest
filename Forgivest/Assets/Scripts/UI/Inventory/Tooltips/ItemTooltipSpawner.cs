using UI.Inventory.Slot;
using UnityEngine;

namespace UI.Inventory.Tooltips
{
    /// <summary>
    /// To be placed on a UI slot to spawn and show the correct item tooltip.
    /// </summary>
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
            if (!itemTooltip) return;

            //var item = GetComponent<InventorySlotUI>().GetItem();

            //itemTooltip.Setup(item);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory.Slot
{
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : ItemIcon
    {
        public override void SetIcon(Sprite item)
        {
            var iconImage = GetComponent<Image>();
            if (item == null)
            {
                iconImage.enabled = false;
            }
            else
            {
                iconImage.enabled = true;
                iconImage.sprite = item;
            }
        }
    }
}
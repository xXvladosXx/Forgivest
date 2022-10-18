using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory.Slot
{
    [RequireComponent(typeof(Image))]
    public abstract class ItemIcon : MonoBehaviour
    {
        public abstract void SetIcon(Sprite item);

        public Sprite GetIcon()
        {
            var iconImage = GetComponent<Image>();
            if (!iconImage.enabled)
            {
                return null;
            }
            return iconImage.sprite;
        }
    }
}
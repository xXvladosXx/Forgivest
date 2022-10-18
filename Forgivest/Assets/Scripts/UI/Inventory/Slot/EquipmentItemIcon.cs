using System;
using UI.Inventory.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory.Slot
{
    [RequireComponent(typeof(Image))]
    public class EquipmentItemIcon : ItemIcon
    {
        private Sprite _startIcon;
        private void Awake()
        {
            _startIcon = GetComponent<Image>().sprite;
        }

        public override void SetIcon(Sprite item)
        {
            var iconImage = GetComponent<Image>();
            if (item == null)
            {
                iconImage.sprite = _startIcon;
            }
            else
            {
                iconImage.enabled = true;
                iconImage.sprite = item;
            }
        }
    }
}
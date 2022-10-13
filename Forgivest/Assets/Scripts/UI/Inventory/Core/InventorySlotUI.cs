using System;
using GameDevTV.UI.Inventories;
using TMPro;
using UnityEngine;
using Utilities.UI.Dragging;

namespace UI.Inventory.Core
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer 
    {
        [field: SerializeField] public InventoryDragItem InventoryDragItem { get; private set; }
        
        [SerializeField] private InventoryItemIcon _itemIcon = null;
        [SerializeField] private TextMeshProUGUI _itemAmount;
        private int _slotIndex;
        private int _currentAmount;

        public int SourceIndex
        {
            get => _slotIndex;
            set => _slotIndex = value;
        }

        public int Index
        {
            get => _slotIndex;
            set => _slotIndex = value;
        }

        public int MaxAcceptable(Sprite item)
        {
            if (GetItem() == null)
            {
                return int.MaxValue;
            }
            return 0;
        }

        public void AddItems(Sprite itemSprite, int number, int index)
        {
            SetItemData(itemSprite, number, index);
        }

        public Sprite GetItem()
        {
            return _itemIcon.GetItem();
        }

        public int GetNumber()
        {
            return _currentAmount;
        }

        public void RemoveItems(int number)
        {
            _itemIcon.SetItem(null);
            _currentAmount = 0;
            _itemAmount.text = "";
        }

        public void SetItemData(Sprite itemSprite, int itemAmount, int index)
        {
            _itemIcon.SetItem(itemSprite);
            _slotIndex = index;
            _currentAmount = itemAmount;
            _itemAmount.text = itemAmount is 1 or 0 ? string.Empty : itemAmount.ToString();
        }
    }
}
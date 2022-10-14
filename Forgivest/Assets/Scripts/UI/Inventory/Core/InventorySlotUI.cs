using System;
using System.Collections.Generic;
using System.Linq;
using GameDevTV.UI.Inventories;
using TMPro;
using UI.Inventory.Dragging;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Inventory.Core
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer
    {
        [field: SerializeField] public InventoryDragItem InventoryDragItem { get; private set; }
        
        [SerializeField] private InventoryItemIcon _itemIcon = null;
        [SerializeField] private TextMeshProUGUI _itemAmount;
        
        private int _slotIndex;
        private int _currentAmount;
        
        public InventoryItemContainerUI Inventory { get; set; }
        
        private Vector3 _startPosition;
        private Transform _originalParent;

        private Canvas _parentCanvas;
      
        public event Action<int, GameObject> OnDragEnded;
        public event Action<int, int> OnItemSwapped; 
        public event Action<int> OnItemSlotChanged; 
        
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
            if (GetIcon() == null)
            {
                return int.MaxValue;
            }
            return 0;
        }

        public void AddItems(Sprite itemSprite, int number, int index)
        {
            SetItemData(itemSprite, number, index);
        }

        public void TryToSwap(IDragDestination destination)
        {
            var destinationSlot = destination as InventorySlotUI;
            var sourceSlot = this;

            destinationSlot = this;
            sourceSlot = destination as InventorySlotUI;
        }

        public Sprite GetIcon()
        {
            return _itemIcon.GetItem();
        }

        public int GetNumber()
        {
            return _currentAmount;
        }

        public void RemoveItems(int number)
        {
            _itemIcon.SetIcon(null);
            _currentAmount = 0;
            _itemAmount.text = "";
        }

        public void SetItemData(Sprite itemSprite, int itemAmount, int index)
        {
            _itemIcon.SetIcon(itemSprite);
            _slotIndex = index;
            _currentAmount = itemAmount;
            _itemAmount.text = itemAmount is 1 or 0 ? string.Empty : itemAmount.ToString();
        }

        public void StopRenderingItem()
        {
            _itemIcon.SetIcon(null);
        }
    }
}
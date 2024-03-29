﻿using System;
using TMPro;
using UI.Inventory.Core;
using UI.Inventory.DragDrop;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Inventory.Slot
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer, IPointerExitHandler,
        IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private ItemIcon _itemIcon = null;
        [SerializeField] private TextMeshProUGUI _itemAmount;
        [SerializeField] private DraggableObject _draggableObject;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        private Vector3 _startPosition;
        private Transform _originalParent;
        private DraggableObject _currentDraggableObject;
        private int _currentAmount;
        private Canvas _parentCanvas;

        public event Action<int, int, Sprite, int, IInventoryHolder> OnItemTryToSwap;
        public event Action<int, Sprite, int, IInventoryHolder> OnItemTryToDrop;

        public int Index { get; private set; }
        public string Name { get; set; }
        public string ItemDescription { get; set; }
        public string Requirements { get; set; }

        private void Awake()
        {
            _parentCanvas = GetComponentInParent<Canvas>();
        }

        public void TryToSwap(IDragDestination destination, IInventoryHolder inventoryHolder)
        {
            _currentDraggableObject.OnDestinationFound -= TryToSwap;
            OnItemTryToSwap?.Invoke(Index, destination.Index, _itemIcon.GetIcon(), _currentAmount, inventoryHolder);
        }

        private void TryToDrop(IInventoryHolder inventoryHolder)
        {
            _currentDraggableObject.OnDestinationEmpty -= TryToDrop;
            OnItemTryToDrop?.Invoke(Index, _itemIcon.GetIcon(), _currentAmount, inventoryHolder);
        }

        public Sprite GetIcon()
        {
            return _itemIcon.GetIcon();
        }

        public virtual void SetSlotData(Sprite itemSprite, int itemAmount, int index,
            string description, string itemName, string requirements)
        {
            _itemIcon.SetIcon(itemSprite);
            Index = index;
            _currentAmount = itemAmount;
            _itemAmount.text = itemAmount is 1 or 0 ? string.Empty : itemAmount.ToString();
            ItemDescription = description;
            Name = itemName;
            Requirements = requirements;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_itemIcon.GetIcon() == null) return;

            var draggableObject = Instantiate(_draggableObject,
                transform.position,
                Quaternion.identity,
                _parentCanvas.transform);
            
            draggableObject.Init(GetIcon(), _currentAmount,
                _parentCanvas, 
                transform, 
                _rectTransform.sizeDelta.x,
                _rectTransform.sizeDelta.y);
            
            _currentDraggableObject = draggableObject;
            _currentDraggableObject.DrawObject(eventData);
            
            _currentDraggableObject.OnDestinationFound += TryToSwap;
            _currentDraggableObject.OnDestinationEmpty += TryToDrop;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_itemIcon.GetIcon() == null) return;
            if(_currentDraggableObject == null) return;

            _currentDraggableObject.DestroyObject(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_itemIcon.GetIcon() == null) return;
            if(_currentDraggableObject == null) return;

            _currentDraggableObject.UpdateObject(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (GetIcon() == null)
            {
                _canvasGroup.blocksRaycasts = false;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}
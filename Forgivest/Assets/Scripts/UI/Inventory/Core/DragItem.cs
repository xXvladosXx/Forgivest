using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory.Core
{
    public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _image;
        private InventorySlotUI _inventorySlotUI;
        private Vector3 _startPosition;
        private Transform _originalParent;
        private Canvas _parentCanvas;

        public void Init(InventorySlotUI inventorySlotUI, Canvas parentCanvas)
        {
            _inventorySlotUI = inventorySlotUI;
            _parentCanvas = parentCanvas;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_inventorySlotUI == null)
                return;
            
            _startPosition = transform.position;
            _originalParent = transform.parent;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            transform.SetParent(_parentCanvas.transform, true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_inventorySlotUI == null)
                return;
            _image.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.position = _startPosition;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            transform.SetParent(_originalParent, true);

            var container = GetContainer(eventData);

            container?.TryToSwap(_inventorySlotUI, _image.sprite);
        }
        
        private IDragDestination GetContainer(PointerEventData eventData)
        {
            if (eventData.pointerEnter)
            {
                var container = eventData.pointerEnter.GetComponentInParent<IDragDestination>();

                return container;
            }
            return null;
        }
    }
    
}
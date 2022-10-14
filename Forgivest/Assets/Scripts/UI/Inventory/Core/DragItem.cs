using System;
using System.Linq;
using UI.Inventory.Dragging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory.Core
{
    public class DragItem : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _image;
        private InventorySlotUI _inventorySlotUI;

        public void Init(InventorySlotUI inventorySlotUI) => this._inventorySlotUI = inventorySlotUI;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if ((UnityEngine.Object) this._inventorySlotUI == (UnityEngine.Object) null)
                return;
            this._inventorySlotUI.StopRenderingItem();
            this._image.sprite = this._inventorySlotUI.GetIcon();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if ((UnityEngine.Object) this._inventorySlotUI == (UnityEngine.Object) null)
                return;
            this._image.transform.position = (Vector3) eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GameObject gameObject = eventData.hovered.FirstOrDefault(destination => destination.TryGetComponent<IDragDestination>(out var _));
            if (! gameObject != null)
                return;
            gameObject.GetComponent<IDragDestination>().TryToSwap((IDragDestination) this._inventorySlotUI);
        }
    }
    
}
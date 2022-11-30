using System;
using TMPro;
using UI.Inventory.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UI.Inventory.DragDrop
{
    [RequireComponent(typeof(Image),
        typeof(CanvasGroup),
    typeof(RectTransform))]
    public class DraggableObject : MonoBehaviour
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _amount;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _parent;
        [SerializeField] private RectTransform _rectTransform;
        
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _amountText;
        
        private float _height;
        private float _width;
        private IInventoryHolder _currentInventoryHolder;
        
        private const float SIZE_MODIFIER = .5f;

        public event Action<IDragDestination, IInventoryHolder> OnDestinationFound; 
        public event Action<IInventoryHolder> OnDestinationEmpty; 

        public void Init(Sprite sprite, 
            int amount, 
            Canvas canvas, 
            Transform parent,
            float width,
            float height)
        {
            _sprite = sprite;
            _amount = amount;
            _canvas = canvas;
            _parent = parent;
            _width = width;
            _height = height;
        }

        public void DrawObject(PointerEventData eventData)
        {
            _image.sprite = _sprite;
            _canvasGroup.blocksRaycasts = false;
            _amountText.text = _amount is 1 or 0 ? string.Empty : _amount.ToString();
            _currentInventoryHolder = GetContainer(eventData);

            _rectTransform.sizeDelta = new Vector2(_width * SIZE_MODIFIER, _height * SIZE_MODIFIER);
            
            transform.SetParent(_canvas.transform, true);
        }
        
        public void UpdateObject(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }
        
        public void DestroyObject(PointerEventData eventData)
        {
            transform.position = _parent.position;
            _canvasGroup.blocksRaycasts = true;
            transform.SetParent(_parent, true);
            
            var destination = GetDestination(eventData);
            if (destination == null)
            {
                OnDestinationEmpty?.Invoke(_currentInventoryHolder);
            }
            else
            {
                OnDestinationFound?.Invoke(destination, _currentInventoryHolder);
            }
                
            
            Destroy(gameObject);
        }

        private IInventoryHolder GetContainer(PointerEventData eventData)
        {
            if (!eventData.pointerEnter) return null;
            
            var container = eventData.pointerEnter.GetComponent<IInventoryHolder>() ?? eventData.pointerEnter.GetComponentInParent<IInventoryHolder>();

            return container;
        }

        private IDragDestination GetDestination(PointerEventData eventData)
        {
            if (!eventData.pointerEnter) return null;
            
            var container = eventData.pointerEnter.GetComponent<IDragDestination>();

            return container;
        }
    }
}
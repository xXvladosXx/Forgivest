using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Inventory.Tooltips
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _tooltipPrefab = null;
        [SerializeField] private RectTransform _rectTransform;
        
        private GameObject _tooltip = null;
        private Canvas _canvas;
        private RectTransform _tooltipRectTransform;

        public abstract void UpdateTooltip(GameObject tooltip);
        public abstract bool CanCreateTooltip();

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
            _tooltip = Instantiate(_tooltipPrefab, _canvas.transform);
            _tooltipRectTransform = _tooltip.GetComponent<RectTransform>();
        }

        private void Start()
        {
            _tooltip.SetActive(false);
        }

        private void OnDisable()
        {
            ClearTooltip();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (_tooltip.activeSelf && !CanCreateTooltip())
            {
                ClearTooltip();
            }

            if (!_tooltip.activeSelf && CanCreateTooltip())
            {
                _tooltip.SetActive(true);
            }

            if (!_tooltip.activeSelf) return;
            
            UpdateTooltip(_tooltip);
            PositionTooltip();
        }

        private void PositionTooltip()
        {
            Canvas.ForceUpdateCanvases();

            var tooltipCorners = new Vector3[4];
            _tooltipRectTransform.GetWorldCorners(tooltipCorners);
            var slotCorners = new Vector3[4];
            _rectTransform.GetWorldCorners(slotCorners);

            bool below = transform.position.y > Screen.height / 2f;
            bool right = transform.position.x < Screen.width / 2f;

            int slotCorner = GetCornerIndex(below, right);
            int tooltipCorner = GetCornerIndex(!below, !right);

            _tooltip.transform.position = slotCorners[slotCorner] - tooltipCorners[tooltipCorner] + _tooltip.transform.position;
        }

        private int GetCornerIndex(bool below, bool right)
        {
            return below switch
            {
                true when !right => 0,
                false when !right => 1,
                false when right => 2,
                _ => 3
            };
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            ClearTooltip();
        }

        private void ClearTooltip()
        {
            if (_tooltip)
            {
                _tooltip.SetActive(false);
            }
        }
    }
}
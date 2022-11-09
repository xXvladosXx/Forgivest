using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Inventory.Tooltips
{
    public class TooltipScreenSpace : MonoBehaviour
    {
        public static TooltipScreenSpace Instance { get; private set; }

        [SerializeField] private RectTransform _canvas;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _backgroundRectTransform;
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        private void Awake()
        {
            Instance = this;
            HideTooltip();
        }

        public void ShowTooltip(string tooltipString)
        {
            SetText(tooltipString);
            _rectTransform.gameObject.SetActive(true);
        }
        
        public void HideTooltip()
        {
            _rectTransform.gameObject.SetActive(false);
        }

        private void SetText(string text)
        {
            _textMeshProUGUI.text = text;
            _textMeshProUGUI.ForceMeshUpdate();

            _rectTransform.ForceUpdateRectTransforms();
        }

        private void Update()
        {
            Vector3 anchoredPosition = Mouse.current.position.ReadValue() / _canvas.localScale.x;

            if (anchoredPosition.x + _backgroundRectTransform.rect.width > _canvas.rect.width)
            {
                anchoredPosition.x = _canvas.rect.width - _backgroundRectTransform.rect.width;
            }

            if (anchoredPosition.y + _backgroundRectTransform.rect.height > _canvas.rect.height)
            {
                anchoredPosition.y = _canvas.rect.height - _backgroundRectTransform.rect.height;
            }

            _rectTransform.anchoredPosition = anchoredPosition;
        }
    }
}
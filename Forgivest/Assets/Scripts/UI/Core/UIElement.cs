using System;
using UnityEngine;

namespace UI.Core
{
    public abstract class UIElement : MonoBehaviour, IUIElement
    {
        public event Action OnElementHide;
        public event Action<IUIElement> OnElementShow;
        public event Action OnCursorShow;
        public event Action OnCursorHide;

        public bool IsActive { get; }
        public string Name { get; }
        public GameObject GameObject { get; }

        private bool _isActive;

        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnElementShow?.Invoke(this);
            OnCursorShow?.Invoke();
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnElementHide?.Invoke();
            OnCursorHide?.Invoke();
        }
    }
}
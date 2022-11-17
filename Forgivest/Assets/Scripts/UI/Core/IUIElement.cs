using System;
using UnityEngine;

namespace UI.Core
{
    public interface IUIElement 
    {
        event Action OnElementHide;
        event Action<IUIElement> OnElementShow;

        bool IsActive { get; }
        string Name { get; }
        GameObject GameObject { get; }

        void Show();
        void Hide();
    }
}
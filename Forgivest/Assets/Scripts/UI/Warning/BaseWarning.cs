using System;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Warning
{
    public class BaseWarning : UIElement
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        public void ShowWarning(string text)
        {
            Show();
            _text.text = text;
        }
    }
}
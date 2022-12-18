using System;
using System.Collections.Generic;
using System.Linq;
using UI.Inventory;
using UI.Inventory.Core;
using UI.Menu;
using UI.Skill;
using UnityEngine;

namespace UI.Core
{
    public class PanelSwitcher : UIElement
    {
        [field: SerializeField] public LoadMenu LoadMenu { get; private set; }
        [SerializeField] private List<Panel> _panels = new List<Panel>();
        private Panel _currentInventoryPanel;

        private void Awake()
        {
            _currentInventoryPanel = _panels[0];
            HideAllUIElements();
        }

        public bool ChangeUIElement<T>(bool withoutSwitching = false) where T : Panel
        {
            if (_currentInventoryPanel != null)
            {
                if (withoutSwitching)
                {
                    HideUIElement();
                    return true;
                }
                
                if (_currentInventoryPanel.GetType() == typeof(T))
                {
                    HideUIElement();
                    return true;
                }

                HideUIElement();
            }

            ShowUIElement<T>();
            return false;
        }

        public T GetPanel<T>() where T : Panel
        {
            return _panels.FirstOrDefault(panel => panel.GetType() == typeof(T)) as T;
        }
        
        private void ShowUIElement<T>() where T : Panel
        {
            var uiElement = _panels.FirstOrDefault(e => e.GetType() == typeof(T));
            if (uiElement != null)
            {
                uiElement.Show();
                _currentInventoryPanel = uiElement;
                _currentInventoryPanel.OnElementHide += HideUIElement;
            }
        }

        public void HideUIElement()
        {
            _currentInventoryPanel.OnElementHide -= HideUIElement;
            _currentInventoryPanel.Hide();
            _currentInventoryPanel = null;
        }
        
        public void HideAllUIElements()
        {
            foreach (var panel in _panels)
            {
                panel.Hide();
                _currentInventoryPanel = null;
            }
        }
    }
}
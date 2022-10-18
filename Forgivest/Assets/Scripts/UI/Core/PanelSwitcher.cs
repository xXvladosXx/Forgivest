using System;
using System.Collections.Generic;
using System.Linq;
using InventoryExample.UI.Skill;
using UI.Inventory;
using UI.Inventory.Core;
using UnityEngine;

namespace UI.Core
{
    public class PanelSwitcher : UIElement
    {
        [SerializeField] private List<Panel> _panels = new List<Panel>();
        private Panel _currentPanel;

        private void Awake()
        {
            _panels = GetComponentsInChildren<Panel>().ToList();
            _currentPanel = _panels[0];
            HideUIElement();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SwitchUIElement<InventoryPanel>();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                SwitchUIElement<SkillPanel>();
            }
        }

        protected void SwitchUIElement<T>() where T : Panel
        {
            if (_currentPanel != null)
            {
                if (_currentPanel.GetType() == typeof(T))
                {
                    HideUIElement();
                    return;
                }

                HideUIElement();
            }

            ShowUIElement<T>();
        }

        private void ShowUIElement<T>() where T : Panel
        {
            var uiElement = _panels.FirstOrDefault(e => e.GetType() == typeof(T));
            if (uiElement != null)
            {
                uiElement.Show();
                _currentPanel = uiElement;
                _currentPanel.OnElementHide += HideUIElement;
            }
        }

        public void HideUIElement()
        {
            _currentPanel.OnElementHide -= HideUIElement;
            _currentPanel.Hide();
            _currentPanel = null;
        }

    }
}
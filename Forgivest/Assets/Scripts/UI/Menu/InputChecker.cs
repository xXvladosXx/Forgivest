using System;
using UI.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace UI.Menu
{
    public class InputChecker : UIElement
    {
        [SerializeField] private GameObject _menu;
        [SerializeField] private GameObject _gameHeader;

        private event Action <InputEventPtr, InputDevice> OnInputEvent;
        private void Awake()
        {
            OnInputEvent = (eventPtr, device) =>
            {
                if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
                    return;
                var controls = device.allControls;
                var buttonPressPoint = InputSystem.settings.defaultButtonPressPoint;
                for (var i = 0; i < controls.Count; ++i)
                {
                    var control = controls[i] as ButtonControl;
                    if (control == null || control.synthetic || control.noisy)
                        continue;
                    if (control.ReadValueFromEvent(eventPtr, out var value) && value >= buttonPressPoint)
                    {
                        HideHeader();
                        break;
                    }
                }
            };

            InputSystem.onEvent += OnInputEvent;
        }

        private void HideHeader()
        {
            InputSystem.onEvent -= OnInputEvent;
                
            _gameHeader.SetActive(false);
            ShowMenu();
        }

        private void ShowMenu()
        {
            _menu.SetActive(true);
        }
    }
}
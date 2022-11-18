using System;
using UI.Core;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace UI.InputChecker
{
    public class InputChecker : UIElement
    {
        private event Action <InputEventPtr, InputDevice> OnInputEvent;

        public event Action OnHidden;
        
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
                        Hide();
                        break;
                    }
                }
            };
        }

        private void OnEnable()
        {
            InputSystem.onEvent += OnInputEvent;
        }

        private void OnDisable()
        {
            InputSystem.onEvent -= OnInputEvent;
        }

        public override void Hide()
        {
            OnHidden?.Invoke();
        }
    }
}
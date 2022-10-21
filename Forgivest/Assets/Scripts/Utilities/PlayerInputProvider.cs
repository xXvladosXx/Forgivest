using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utilities
{
    public class PlayerInputProvider : MonoBehaviour
    {
        public PlayerControlls PlayerInput { get; private set; }
        public PlayerControlls.MainActions PlayerMainActions { get; private set; }

        public void Awake()
        {
            PlayerInput = new PlayerControlls();

            PlayerMainActions = PlayerInput.Main;
        }

        private void OnEnable()
        {
            PlayerInput.Enable();
        }
       
        private void OnDisable()
        {
            PlayerInput.Disable();
        }

        public Vector2 ReadMousePosition() =>
            PlayerMainActions.Mouse.ReadValue<Vector2>();
        
        private IEnumerator DisableAction(InputAction action, float seconds)
        {
            action.Disable();

            yield return new WaitForSeconds(seconds);

            action.Enable();
        }
    }
}
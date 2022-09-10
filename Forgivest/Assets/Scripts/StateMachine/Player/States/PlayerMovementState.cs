using System;
using AnimatorStateMachine.StateMachine;
using Data.Player;
using Enemy;
using Interaction.Core;
using StateMachine.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Characters.Player.StateMachines.Movement.States
{
    public class PlayerMovementState : IState
    {
        protected PlayerStateMachine PlayerStateMachine { get; private set; }

        protected AliveEntityGroundedData GroundedData { get; private set; }

        public PlayerMovementState(PlayerStateMachine playerPlayerStateMachine)
        {
            PlayerStateMachine = playerPlayerStateMachine;

            GroundedData = PlayerStateMachine.AliveEntityStateData.GroundedData;
        }

        public virtual void Enter()
        {
            AddInputActionsCallbacks();
        }

        public virtual void Exit()
        {
            RemoveInputActionsCallbacks();
        }

        public virtual void Update()
        {
            if (PlayerStateMachine.PlayerInputProvider.PlayerMainActions.Action.IsPressed())
            {
                OnPressedMouseButton();
            }

            UpdateMovementAnimation();
            MovementInput();
        }

        public void FixedUpdate()
        {
        }

        protected virtual void AddInputActionsCallbacks()
        {
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.Action.performed += OnClickPerformed;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.Action.canceled += OnClickCanceled;
        }

        protected virtual void RemoveInputActionsCallbacks()
        {
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.Action.performed -= OnClickPerformed;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.Action.canceled -= OnClickCanceled;
        }

        private void MovementInput()
        {
            if (!TryToGetHitRaycast(out var raycastHit)) return;

            //if mouse button is not pressed dont move
            if (!PlayerStateMachine.ReusableData.ShouldMove)
            {
                //when mouse was canceled stop movement
                if (ShouldStop()) return;

                return;
            }

            PlayerStateMachine.ReusableData.ClickedPoint = raycastHit.Value.point;
            raycastHit.Value.collider.TryGetComponent(out IInteractable interactable);
            PlayerStateMachine.ReusableData.InteractableObject = interactable;

            if (ShouldStop()) return;
            PlayerStateMachine.Movement.MoveTo(raycastHit.Value.point, GetMovementSpeed());
        }

        public virtual void OnAnimationEnterEvent()
        {
        }

        public virtual void OnAnimationExitEvent()
        {
        }

        public virtual void OnAnimationTransitionEvent()
        {
        }

        protected virtual void OnClickCanceled(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ReusableData.ShouldMove = false;
        }

        protected virtual void OnClickPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ReusableData.ShouldMove = true;
        }

        protected void ResetVelocity()
        {
            PlayerStateMachine.Movement.ResetVelocity();
        }

        private float GetMovementSpeed() =>
            GroundedData.BaseSpeed * PlayerStateMachine.ReusableData.MovementSpeedModifier;

        protected virtual void OnPressedMouseButton()
        {
        }

        protected virtual void OnStop()
        {
        }

        protected bool TryToGetHitRaycast(out RaycastHit? raycastHit)
        {
            raycastHit = PlayerStateMachine.RaycastUser.RaycastExcept(
                PlayerStateMachine.PlayerInputProvider.ReadMousePosition(),
                LayerUtils.Player);

            return raycastHit != null;
        }

        private void UpdateMovementAnimation()
        {
            Vector3 velocity = PlayerStateMachine.Movement.GetNavMeshVelocity();
            Vector3 localVelocity = PlayerStateMachine.Movement.Transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;

            PlayerStateMachine.AnimationChanger.UpdateBlendAnimation(
                PlayerStateMachine.AnimationData.SpeedParameterHash,
                speed, .1f);
        }

        protected bool ShouldStop()
        {
            if (PlayerStateMachine.Movement.GetDistanceToPoint(
                    PlayerStateMachine.ReusableData.ClickedPoint) <
                PlayerStateMachine.ReusableData.StoppingDistance)
            {
                Stop();
                return true;
            }

            return false;
        }

        private void Stop()
        {
            PlayerStateMachine.Movement.Stop();
            OnStop();
        }
    }
}
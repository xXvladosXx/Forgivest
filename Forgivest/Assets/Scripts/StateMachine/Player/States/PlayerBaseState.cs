using AnimatorStateMachine.StateMachine;
using Data.Player;
using Enemy;
using Interaction.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace StateMachine.Player.States
{
    public class PlayerBaseState : IState
    {
        protected PlayerStateMachine PlayerStateMachine { get; private set; }

        protected AliveEntityGroundedData GroundedData { get; private set; }

        public PlayerBaseState(PlayerStateMachine playerPlayerStateMachine)
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
                CheckRaycast();
                OnInteractableCheck();
                OnClickPressed();
            }

            UpdateMovementAnimation();
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

        private void CheckRaycast()
        {
            if (!TryToGetHitRaycast(out var raycastHit)) return;

            PlayerStateMachine.ReusableData.RaycastClickedPoint = raycastHit.Value.point;
            raycastHit.Value.collider.TryGetComponent(out IInteractable interactable);
            PlayerStateMachine.ReusableData.InteractableObject = interactable;
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
        }

        protected virtual void OnClickPerformed(InputAction.CallbackContext obj)
        {
        }
        
        protected virtual void OnClickPressed()
        {
            
        }

        protected float GetMovementSpeed() =>
            GroundedData.BaseSpeed * PlayerStateMachine.ReusableData.MovementSpeedModifier;

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

        protected virtual void OnInteractableCheck()
        {
            switch (PlayerStateMachine.ReusableData.InteractableObject)
            {
                case EnemyEntity enemyEntity:
                    PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerAggroState);
                    return;
            }
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
                    PlayerStateMachine.ReusableData.RaycastClickedPoint) <
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
        }
    }
}
using AnimatorStateMachine.StateMachine;
using Data.Player;
using Enemy;
using Interaction.Core;
using RaycastSystem.Core;
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
            CheckRaycastNonClicked();
            if (PlayerStateMachine.PlayerInputProvider.PlayerMainActions.Action.IsPressed())
            {
                if (CheckRaycast())
                {
                    OnInteractableCheck();
                    OnClickPressed();
                }
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
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.FirstSkill.performed += OnFirstSkillPerformed;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.SecondSkill.performed += OnSecondSkillPerformed;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.ThirdSkill.performed += OnThirdSkillPerformed;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.FourthSkill.performed += OnFourthSkillPerformed;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.FifthSkill.performed += OnFifthSkillPerformed;
        }

        protected virtual void RemoveInputActionsCallbacks()
        {
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.Action.performed -= OnClickPerformed;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.Action.canceled -= OnClickCanceled;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.FirstSkill.performed -= OnFirstSkillPerformed;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.SecondSkill.performed -= OnSecondSkillPerformed;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.ThirdSkill.performed -= OnThirdSkillPerformed;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.FourthSkill.performed -= OnFourthSkillPerformed;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.FifthSkill.performed -= OnFifthSkillPerformed;
        }

        private bool CheckRaycastNonClicked()
        {
            if (!PlayerStateMachine.RaycastUser.RaycastHit.HasValue) return false;

            PlayerStateMachine.RaycastUser.RaycastHit.Value.collider.TryGetComponent(out IRaycastable raycastable);
            PlayerStateMachine.ReusableData.Raycastable = raycastable;

            return true;
        }
        
        private bool CheckRaycast()
        {
            if (!PlayerStateMachine.RaycastUser.RaycastHit.HasValue) return false;

            PlayerStateMachine.ReusableData.RaycastClickedPoint = PlayerStateMachine.RaycastUser.RaycastHit.Value.point;
            PlayerStateMachine.RaycastUser.RaycastHit.Value.collider.TryGetComponent(out IInteractable interactable);
            PlayerStateMachine.ReusableData.InteractableObject = interactable;

            return true;
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
        
        protected virtual void OnInteractableCheck()
        {
             switch (PlayerStateMachine.ReusableData.InteractableObject)
            {
                case EnemyEntity enemyEntity:
                     PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerChasingState);
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
        
        protected bool GetDistanceTo(Vector3 target)
        {
            if (PlayerStateMachine.Movement.GetDistanceToPoint(target) < PlayerStateMachine.ReusableData.StoppingDistance)
            {
                Stop();
                return true;
            }

            return false;
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
        
        protected virtual void OnFifthSkillPerformed(InputAction.CallbackContext obj)
        {
            
        }

        protected virtual void OnFourthSkillPerformed(InputAction.CallbackContext obj)
        {
            
        }

        protected virtual void OnThirdSkillPerformed(InputAction.CallbackContext obj)
        {
            
        }

        protected virtual void OnSecondSkillPerformed(InputAction.CallbackContext obj)
        {
            
        }
        protected virtual void OnFirstSkillPerformed(InputAction.CallbackContext obj)
        {
            
        }
    }
}
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
            PlayerStateMachine = playerPlayerStateMachine;
            
            GroundedData = PlayerStateMachine.Player.StateData.GroundedData;
        }

        public virtual void Enter()
        {
            AddInputActionsCallbacks();
        }

        public virtual void Exit()
        {
            RemoveInputActionsCallbacks();
        }

        public virtual void HandleInput()
        {
        }

        public virtual void Update()
        {
            UpdateMovementAnimation();           
            
            MovementInput();
        }

        public void FixedUpdate()
        {
            
        }

        protected virtual void AddInputActionsCallbacks()
        {
            PlayerStateMachine.Player.PlayerInputProvider.PlayerMainActions.Action.performed += OnClickPerformed;
            PlayerStateMachine.Player.PlayerInputProvider.PlayerMainActions.Action.canceled += OnClickCanceled;
        }

        protected virtual void RemoveInputActionsCallbacks()
        {
            PlayerStateMachine.Player.PlayerInputProvider.PlayerMainActions.Action.performed -= OnClickPerformed;
            PlayerStateMachine.Player.PlayerInputProvider.PlayerMainActions.Action.canceled -= OnClickCanceled;
        }
        
        private void MovementInput()
        {
            RaycastHit raycastHit;
            var ray = PlayerStateMachine.Player.Camera.ScreenPointToRay(ReadMousePosition());
            bool hasHit = Physics.Raycast(ray, out raycastHit, Mathf.Infinity);
            if (!hasHit) return;

            if (raycastHit.collider.gameObject.layer == PlayerStateMachine.Player.LayerData.UninteractableLayer)
            {
                Stop();
                return;
            }
            //if mouse button is not pressed dont move
            if (!PlayerStateMachine.ReusableData.ShouldMove)
            {
                //when mouse was canceled stop movement
                if (ShouldStop()) return;

                return;
            }
            
            
            PlayerStateMachine.ReusableData.LastClickedPoint = raycastHit.point;
            raycastHit.collider.TryGetComponent(out IInteractable interactable);
            PlayerStateMachine.ReusableData.InteractableObject = interactable;

            if(ShouldStop()) return;
            Debug.Log("moving");

            StartMoveTo(raycastHit.point);
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
    
        protected void StartAnimation(int animationHash)
        {
            PlayerStateMachine.Player.Animator.SetBool(animationHash, true);
        }

        protected void StopAnimation(int animationHash)
        {
            PlayerStateMachine.Player.Animator.SetBool(animationHash, false);
        }


        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
        }
       
        protected virtual void OnClickCanceled(InputAction.CallbackContext obj)
        {
            OnMove();
            PlayerStateMachine.ReusableData.ShouldMove = false;
        }

        protected virtual void OnClickPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ReusableData.ShouldMove = true;
            OnMove();
        }
        
        protected void ResetVelocity()
        {
            PlayerStateMachine.Player.Rigidbody.velocity = Vector3.zero;
        }
        
        protected float GetMovementSpeed() => GroundedData.BaseSpeed * PlayerStateMachine.ReusableData.MovementSpeedModifier;
   
        protected virtual void OnMove()
        {
           
        }
        
        private void UpdateMovementAnimation()
        {
            Vector3 velocity = PlayerStateMachine.Player.NavMeshAgent.velocity;
            Vector3 localVelocity = PlayerStateMachine.Player.transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
        
            PlayerStateMachine.Player.Animator.SetFloat("Speed",
                speed, .1f, Time.deltaTime);
        }
        
        private Vector2 ReadMousePosition() => PlayerStateMachine.Player.PlayerInputProvider.PlayerMainActions.Mouse.ReadValue<Vector2>();

        private void StartMoveTo(Vector3 destination)
        {
            PlayerStateMachine.Player.NavMeshAgent.speed = GetMovementSpeed();
            PlayerStateMachine.Player.NavMeshAgent.destination = destination;
            PlayerStateMachine.Player.NavMeshAgent.isStopped = false;    
        }

        private bool ShouldStop()
        {
            if (Vector3.Distance(PlayerStateMachine.Player.transform.position,
                    PlayerStateMachine.ReusableData.LastClickedPoint) <
                PlayerStateMachine.ReusableData.StoppingDistance)
            {
                Stop();
                return true;
            }

            return false;
        }
        
        private void Stop()
        {
            PlayerStateMachine.Player.NavMeshAgent.isStopped = true;
        }
    }
}
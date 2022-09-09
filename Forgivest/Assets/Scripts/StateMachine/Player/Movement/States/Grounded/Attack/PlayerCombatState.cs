using Characters.Player.StateMachines.Movement.States.Grounded;
using Interaction.Core;
using UnityEngine;

namespace StateMachine.Player.StateMachines.Movement.States.Grounded.Attack
{
    public class PlayerCombatState : PlayerGroundedState
    {
        public PlayerCombatState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            PlayerStateMachine.ReusableData.AttackRate = 1;

            base.Enter();
            
            StartAnimation(PlayerStateMachine.Player.AnimationData.AttackParameterHash);
        }

        public override void Update()
        {
            base.Update();
            
            PlayerStateMachine.ReusableData.AttackRate = Mathf.Clamp(
                PlayerStateMachine.ReusableData.AttackRate - Time.deltaTime, 0,
                PlayerStateMachine.ReusableData.AttackRate);
        }

        public override void OnAnimationExitEvent()
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerAggroState);
        }

        protected override void OnPressedMouseButton()
        {
            TryToGetHitRaycast(out var raycastHit);

            PlayerStateMachine.ReusableData.ClickedPoint = raycastHit.point;
            raycastHit.collider.TryGetComponent(out IInteractable interactable);
            
            if(interactable == PlayerStateMachine.ReusableData.InteractableObject)
                return;
            
            base.OnPressedMouseButton();
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(PlayerStateMachine.Player.AnimationData.AttackParameterHash);
        }
    }
}
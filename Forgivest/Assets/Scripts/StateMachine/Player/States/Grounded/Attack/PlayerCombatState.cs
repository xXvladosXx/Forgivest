using Characters.Player.StateMachines.Movement.States.Grounded;
using Interaction.Core;
using UnityEngine;

namespace StateMachine.Player.StateMachines.Movement.States.Grounded.Attack
{
    public class PlayerCombatState : PlayerAttackState
    {
        public PlayerCombatState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            PlayerStateMachine.ReusableData.AttackRate = 1;

            base.Enter();
            
            PlayerStateMachine.AnimationChanger.StartAnimation(PlayerStateMachine.AnimationData.AttackParameterHash); 
        }

        public override void OnAnimationExitEvent()
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerAggroState);
        }

        protected override void OnPressedMouseButton()
        {
            TryToGetHitRaycast(out var raycastHit);

            PlayerStateMachine.ReusableData.ClickedPoint = raycastHit.Value.point;
            raycastHit.Value.collider.TryGetComponent(out IInteractable interactable);
            
            if(interactable == PlayerStateMachine.ReusableData.InteractableObject)
                return;
            
            base.OnPressedMouseButton();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerStateMachine.AnimationChanger.StopAnimation(PlayerStateMachine.AnimationData.AttackParameterHash); 
        }
    }
}
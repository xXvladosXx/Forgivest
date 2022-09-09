using Enemy;
using StateMachine.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded
{
    public class PlayerGroundedState : PlayerMovementState
    {
        public PlayerGroundedState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }
        
      

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.DashingState);
        }

        protected override void OnMove()
        {
            if (PlayerStateMachine.ReusableData.InteractableObject != null)
            {
                switch (PlayerStateMachine.ReusableData.InteractableObject)
                {
                    case EnemyEntity enemyEntity:
                        PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerAttackState);
                        return;
                }
            }
            
            PlayerStateMachine.ChangeState(PlayerStateMachine.RunningState);
        }
    }
}
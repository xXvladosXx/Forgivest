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
        
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

           // PlayerStateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;
            
        }

        public override void Update()
        {
            base.Update();

            if (PlayerStateMachine.ReusableData.InteractableObject != null)
            {
                switch (PlayerStateMachine.ReusableData.InteractableObject)
                {
                    case EnemyEntity enemyEntity:
                        PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerAttackState);
                        break;
                }
            }
            
        }


        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

           // PlayerStateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;
        }

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.DashingState);
        }

        protected virtual void OnMove()
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.RunningState);
        }
    }
}
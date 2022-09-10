using Characters.Player.StateMachines.Movement.States.Grounded;
using UnityEngine;

namespace StateMachine.Player.StateMachines.Movement.States.Grounded
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        public override void Enter()
        {
            PlayerStateMachine.ReusableData.MovementSpeedModifier = 0f;
            PlayerStateMachine.Movement.SetStoppingDistance(0);

            base.Enter();

            ResetVelocity();
        }

        public PlayerIdlingState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }
    }
}
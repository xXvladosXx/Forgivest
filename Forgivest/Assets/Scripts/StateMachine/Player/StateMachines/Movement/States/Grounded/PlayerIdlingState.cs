using Characters.Player.StateMachines.Movement.States.Grounded;
using UnityEngine;

namespace StateMachine.Player.StateMachines.Movement.States.Grounded
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        public override void Enter()
        {
            PlayerStateMachine.ReusableData.MovementSpeedModifier = 0f;
            PlayerStateMachine.Player.NavMeshAgent.stoppingDistance = 0f;

            base.Enter();

            ResetVelocity();
        }

        public override void Update()
        {
            base.Update();
            if (!PlayerStateMachine.ReusableData.ShouldMove) 
                 return;

            OnMove();
        }

        public PlayerIdlingState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }
    }
}
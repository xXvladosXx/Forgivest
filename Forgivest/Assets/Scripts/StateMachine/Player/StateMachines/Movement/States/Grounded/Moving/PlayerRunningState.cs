using Characters.Player.StateMachines.Movement.States.Grounded.Moving;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Movement.States.Grounded.Moving
{
    public class PlayerRunningState : PlayerMovingState
    {
        private float _startTime;

        public override void Enter()
        {
            PlayerStateMachine.ReusableData.MovementSpeedModifier = GroundedData.RunData.SpeedModifier;
            PlayerStateMachine.ReusableData.StoppingDistance = GroundedData.RunData.StoppingDistance;

            base.Enter();
        }


        protected override void OnStop()
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
        }


        public PlayerRunningState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }
    }
}
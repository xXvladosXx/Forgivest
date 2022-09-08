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

            PlayerStateMachine.ReusableData.MaxSmoothModifier = GroundedData.RunData.SmoothInputSpeed;
        }

        public override void Update()
        {
            base.Update();
            
            if(PlayerStateMachine.Player.NavMeshAgent.isStopped)
                PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
        }

        public PlayerRunningState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }
    }
}
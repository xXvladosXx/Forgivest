namespace StateMachine.Player.States.Moving
{
    public class PlayerRunningState : PlayerBaseState
    {
        private float _startTime;

        public override void Enter()
        {
            PlayerStateMachine.ReusableData.MovementSpeedModifier = GroundedData.RunData.SpeedModifier;
            PlayerStateMachine.ReusableData.StoppingDistance = GroundedData.RunData.StoppingDistance;

            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            
            if (ShouldStop())
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
            }
        }

        protected override void OnClickPressed()
        {
            PlayerStateMachine.Movement.MoveTo(PlayerStateMachine.ReusableData.RaycastClickedPoint, GetMovementSpeed());
        }

        public PlayerRunningState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }
    }
}
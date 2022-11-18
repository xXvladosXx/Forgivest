using Player.StateMachine.Player;

namespace Player.States
{
    public class PlayerIdlingState : PlayerAliveState
    {
        public override void Enter()
        {
            PlayerStateMachine.ReusableData.MovementSpeedModifier = 0f;
            PlayerStateMachine.Movement.SetStoppingDistance(0);

            base.Enter();
        }

        protected override void OnClickPressed()
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.RunningState);
        }

        public PlayerIdlingState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }
    }
}
using Player.StateMachine.Player;

namespace Player.States
{
    public class PlayerDeadState : PlayerBaseState
    {
        public PlayerDeadState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            RemoveInputActionsCallbacks();
        }

        protected override void OnInteractableCheck()
        {
        }

        protected override void OnClickPressed()
        {
        }
    }
}
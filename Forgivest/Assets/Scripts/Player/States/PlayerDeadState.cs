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
            
            PlayerStateMachine.AnimationChanger.StartAnimation(PlayerStateMachine.AnimationData.DeathParameterHash);

            RemoveInputActionsCallbacks();
        }

        public override void Exit()
        {
            base.Exit();
            PlayerStateMachine.AnimationChanger.StopAnimation(PlayerStateMachine.AnimationData.DeathParameterHash);
        }

        protected override void OnInteractableCheck()
        {
        }

        protected override void OnClickPressed()
        {
        }
    }
}
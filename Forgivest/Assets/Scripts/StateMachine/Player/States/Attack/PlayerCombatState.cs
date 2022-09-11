namespace StateMachine.Player.States.Attack
{
    public class PlayerCombatState : PlayerAttackState
    {
        public PlayerCombatState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            PlayerStateMachine.ReusableData.AttackRate = 1;

            base.Enter();
            
            PlayerStateMachine.AnimationChanger.StartAnimation(PlayerStateMachine.AnimationData.AttackParameterHash); 
        }

        public override void OnAnimationExitEvent()
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerAggroState);
        }

        protected override void OnInteractableCheck()
        {
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerStateMachine.AnimationChanger.StopAnimation(PlayerStateMachine.AnimationData.AttackParameterHash); 
        }
    }
}
using Player.States.Attack;

namespace StateMachine.Player.States.Attack
{
    public class PlayerCombatState : PlayerAttackState
    {
        public PlayerCombatState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            PlayerStateMachine.ReusableData.AttackRate = PlayerStateMachine.AttackApplier.Weapon.AttackRate;

            base.Enter();
            
            PlayerStateMachine.AnimationChanger.StartAnimation(PlayerStateMachine.AnimationData.AttackParameterHash); 
        }

        public override void OnAnimationExitEvent()
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerChasingState);
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
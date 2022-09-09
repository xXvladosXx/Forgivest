namespace StateMachine.Player.StateMachines.Movement.States.Grounded.Attack
{
    public class PlayerCombatState : PlayerAttackState
    {
        public PlayerCombatState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            StartAnimation(PlayerStateMachine.Player.AnimationData.AirAttackParameterHash);
        }
    }
}
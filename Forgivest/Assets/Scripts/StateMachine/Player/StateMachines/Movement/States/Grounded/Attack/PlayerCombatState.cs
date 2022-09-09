using Characters.Player.StateMachines.Movement.States.Grounded;

namespace StateMachine.Player.StateMachines.Movement.States.Grounded.Attack
{
    public class PlayerCombatState : PlayerGroundedState
    {
        public PlayerCombatState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            StartAnimation(PlayerStateMachine.Player.AnimationData.AttackParameterHash);
        }

        public override void Update()
        {
            base.Update();

            if (PlayerStateMachine.ReusableData.InteractableObject == null)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
            }
        }

        public override void Exit()
        {
            base.Enter();
            
            StopAnimation(PlayerStateMachine.Player.AnimationData.AttackParameterHash);
        }
    }
}
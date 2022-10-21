using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using StateMachine.Player;

namespace Player.States.Skill
{
    public class PlayerCastState : PlayerAliveState
    {
        public PlayerCastState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            PlayerStateMachine.AbilityController.OnAbilityActivated += OnAbilityActivated;
            PlayerStateMachine.AnimationChanger.StartAnimation(PlayerStateMachine.AnimationData.SkillParameterHash);
        }

        private void OnAbilityActivated(ActiveAbility activeAbility)
        {
            
        }

        public override void OnAnimationExitEvent()
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
        }

        public override void Exit()
        {
            base.Exit();

            PlayerStateMachine.AbilityController.OnAbilityActivated -= OnAbilityActivated;
            PlayerStateMachine.AnimationChanger.StopAnimation(PlayerStateMachine.AnimationData.SkillParameterHash);
        }
    }
}

using StateMachine.Player;
using UnityEngine;

namespace Player.States.Skill.Skills
{
    public class PlayerFirstCastState : PlayerCastState
    {
        public PlayerFirstCastState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            if (PlayerStateMachine.ReusableData.Raycastable != null)
            {
                PlayerStateMachine.AbilityController.TryActiveAbility(0,
                    PlayerStateMachine.ReusableData.Raycastable.GameObject);    
                Debug.Log("PlayerCastFirstState");
                PlayerStateMachine.AnimationChanger.StartAnimation(PlayerStateMachine.AnimationData.FirstSkillParameterHash);
            }
            else
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
            }
        }

        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();
            
            PlayerStateMachine.AnimationChanger.StopAnimation(PlayerStateMachine.AnimationData.FirstSkillParameterHash); 
        }
    }
}
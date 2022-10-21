using StateMachine.Player;
using UnityEngine;

namespace Player.States.Skill.Skills
{
    public class PlayerFourthCastState : PlayerCastState
    {
        public PlayerFourthCastState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
            Debug.Log("Fourth Cast");
            PlayerStateMachine.AnimationChanger.StartAnimation(PlayerStateMachine.AnimationData.FourthSkillParameterHash); 
        }
    }
}
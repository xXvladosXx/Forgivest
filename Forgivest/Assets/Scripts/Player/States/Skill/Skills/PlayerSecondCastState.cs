using StateMachine.Player;
using UnityEngine;

namespace Player.States.Skill.Skills
{
    public class PlayerSecondCastState : PlayerCastState
    {
        public PlayerSecondCastState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
            Debug.Log("Second Cast");
            PlayerStateMachine.AnimationChanger.StartAnimation(PlayerStateMachine.AnimationData.SecondSkillParameterHash); 
        }
    }
}
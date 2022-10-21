using StateMachine.Player;
using UnityEngine;

namespace Player.States.Skill.Skills
{
    public class PlayerFifthCastState : PlayerCastState
    {
        public PlayerFifthCastState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            Debug.Log("Fifth Cast");
            PlayerStateMachine.AnimationChanger.StartAnimation(PlayerStateMachine.AnimationData.FifthSkillParameterHash); 
        }
    }
}
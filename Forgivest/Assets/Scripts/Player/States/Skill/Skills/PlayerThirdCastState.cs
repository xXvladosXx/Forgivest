﻿using StateMachine.Player;
using UnityEngine;

namespace Player.States.Skill.Skills
{
    public class PlayerThirdCastState : PlayerCastState
    {
        public PlayerThirdCastState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
            Debug.Log("Third Cast");
            PlayerStateMachine.AnimationChanger.StartAnimation(PlayerStateMachine.AnimationData.ThirdSkillParameterHash); 
        }
    }
}
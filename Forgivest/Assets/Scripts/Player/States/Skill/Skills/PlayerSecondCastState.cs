using System;
using AbilitySystem.AbilitySystem.Runtime.Abilities;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using Player.StateMachine.Player;
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

            TryToActivateSkill(1);
        }
    }
}
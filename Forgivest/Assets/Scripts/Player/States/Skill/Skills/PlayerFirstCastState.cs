using System;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using Player.StateMachine.Player;
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

            TryToActivateSkill(0);
        }
    }
}
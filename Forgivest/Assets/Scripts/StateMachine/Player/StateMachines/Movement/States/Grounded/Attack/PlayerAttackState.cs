using System;
using Characters.Player.StateMachines.Movement.States;
using Characters.Player.StateMachines.Movement.States.Grounded;
using Enemy;
using UnityEngine;

namespace StateMachine.Player.StateMachines.Movement.States.Grounded.Attack
{
    public class PlayerAttackState : PlayerGroundedState
    {
        public PlayerAttackState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            PlayerStateMachine.ReusableData.StoppingDistance = GroundedData.AttackData.StoppingDistance;

            base.Enter();
        }

        protected override void OnStop()
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerCombatState);
        }
    }
}
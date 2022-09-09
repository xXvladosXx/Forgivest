using System;
using Characters.Player.StateMachines.Movement.States;
using Characters.Player.StateMachines.Movement.States.Grounded;
using Enemy;
using Interaction.Core;
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

        public override void Update()
        {
            base.Update();

            PlayerStateMachine.ReusableData.AttackRate = Mathf.Clamp(
                PlayerStateMachine.ReusableData.AttackRate - Time.deltaTime, 0,
                PlayerStateMachine.ReusableData.AttackRate);
        }

        protected override void OnMove()
        {
            
                
            base.OnMove();
        }

        protected override void OnStop()
        {
            if(PlayerStateMachine.ReusableData.AttackRate <= 0)
                PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerCombatState);   
        }
    }
}
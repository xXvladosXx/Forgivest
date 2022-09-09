using System;
using Characters.Player.StateMachines.Movement.States;
using Characters.Player.StateMachines.Movement.States.Grounded;
using Enemy;
using Interaction.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Movement.States.Grounded.Attack
{
    public class PlayerAggroState : PlayerGroundedState
    {
        public PlayerAggroState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
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

        protected override void OnClickPerformed(InputAction.CallbackContext obj)
        {
            base.OnClickPerformed(obj);
            
            if(ShouldStop())
                PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerCombatState);   
        }

        protected override void OnStop()
        {
            if(PlayerStateMachine.ReusableData.AttackRate <= 0)
                PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerCombatState);   
        }
    }
}
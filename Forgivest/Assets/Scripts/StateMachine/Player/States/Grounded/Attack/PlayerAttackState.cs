using Characters.Player.StateMachines.Movement.States.Grounded;
using UnityEngine;

namespace StateMachine.Player.StateMachines.Movement.States.Grounded.Attack
{
    public class PlayerAttackState : PlayerGroundedState
    {
        public PlayerAttackState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }
        
        public override void Update()
        {
            base.Update();

            PlayerStateMachine.Rotator.RotateRigidbody(PlayerStateMachine.ReusableData.ClickedPoint, 100);
            
            PlayerStateMachine.ReusableData.AttackRate = Mathf.Clamp(
                PlayerStateMachine.ReusableData.AttackRate - Time.deltaTime, 0,
                PlayerStateMachine.ReusableData.AttackRate);
        }
    }
}
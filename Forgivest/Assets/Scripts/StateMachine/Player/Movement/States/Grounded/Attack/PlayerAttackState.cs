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

            var lookRotation = Quaternion.LookRotation(PlayerStateMachine.ReusableData.ClickedPoint -
                                                       PlayerStateMachine.Player.transform.position);
            
            PlayerStateMachine.Player.Rigidbody.MoveRotation(
                Quaternion.Slerp(PlayerStateMachine.Player.transform.rotation,
                    lookRotation, Time.deltaTime*100));
            
            PlayerStateMachine.ReusableData.AttackRate = Mathf.Clamp(
                PlayerStateMachine.ReusableData.AttackRate - Time.deltaTime, 0,
                PlayerStateMachine.ReusableData.AttackRate);
        }
    }
}
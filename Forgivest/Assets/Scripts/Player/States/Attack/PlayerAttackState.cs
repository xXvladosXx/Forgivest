using Player.StateMachine.Player;
using UnityEngine;

namespace Player.States.Attack
{
    public class PlayerAttackState : PlayerAliveState
    {
        public PlayerAttackState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            PlayerStateMachine.ReusableData.StoppingDistance = GroundedData.AttackData.StoppingDistance;
        }

        public override void Update()
        {
            base.Update();

            PlayerStateMachine.Rotator.RotateRigidbody(PlayerStateMachine.ReusableData.RaycastClickedPoint, 100);
            
            PlayerStateMachine.ReusableData.AttackRate = Mathf.Clamp(
                PlayerStateMachine.ReusableData.AttackRate - Time.deltaTime, 0,
                PlayerStateMachine.ReusableData.AttackRate);
        }

        protected override void OnClickPressed()
        {
            if (PlayerStateMachine.ReusableData.InteractableObject == null)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.RunningState);
            }
        }
    }
}
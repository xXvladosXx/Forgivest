using AttackSystem.Core;
using Player.StateMachine.Player;
using UnityEngine.InputSystem;

namespace Player.States
{
    public class PlayerAliveState : PlayerBaseState
    {
        public PlayerAliveState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            PlayerStateMachine.DamageHandler.OnDied += MakePlayerDead;
        }

        public override void Exit()
        {
            base.Exit();
            PlayerStateMachine.DamageHandler.OnDied -= MakePlayerDead;
        }

        protected override void OnFirstSkillPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerFirstCastState);
        }

        protected override void OnSecondSkillPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerSecondCastState);
        }

        protected override void OnThirdSkillPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerThirdCastState);
        }

        protected override void OnFourthSkillPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerFourthCastState);
        }

        protected override void OnFifthSkillPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerFifthCastState);
        }

        private void MakePlayerDead(AttackData attackData)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerDeadState);
        }
    }
}
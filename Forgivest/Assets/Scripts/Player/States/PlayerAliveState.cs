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
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.Z.performed += OnFirstPotionUse;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.X.performed += OnSecondPotionUse;
        }

        private void OnSecondPotionUse(InputAction.CallbackContext obj)
        {
            UsePotion(6);
        }

        private void OnFirstPotionUse(InputAction.CallbackContext obj)
        {
            UsePotion(5);
        }

        private void UsePotion(int index)
        {
            var potion = PlayerStateMachine.AbilityHandler.Hotbar.ItemContainer.Slots[index].Item;
            if (potion.TryToUseItem(PlayerStateMachine.StatController))
            {
                PlayerStateMachine.AbilityHandler.Hotbar.ItemContainer.Remove(this, index);
            }
        }

        public override void Exit()
        {
            base.Exit();
            PlayerStateMachine.DamageHandler.OnDied -= MakePlayerDead;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.Z.performed -= OnFirstPotionUse;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.X.performed -= OnSecondPotionUse;
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
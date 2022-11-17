using Player.States.Attack;

namespace StateMachine.Player.States.Attack
{
    public class PlayerCombatState : PlayerAttackState
    {
        public PlayerCombatState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            PlayerStateMachine.ReusableData.AttackRate = PlayerStateMachine.AttackApplier.CurrentWeapon.AttackRate;

            base.Enter();

            PlayerStateMachine.Rotator.RotateRigidbody(
                PlayerStateMachine.ReusableData.InteractableObject.GameObject.transform.position, 100);
            PlayerStateMachine.AnimationChanger.StartAnimation(PlayerStateMachine.AnimationData.AttackParameterHash);

            PlayerStateMachine.ReusableData.InteractableObject.OnDestroyed += OnInteractableObjectDestroyed;
        }

        private void OnInteractableObjectDestroyed()
        {
            if (PlayerStateMachine.ReusableData.InteractableObject != null)
            {
                PlayerStateMachine.ReusableData.InteractableObject.OnDestroyed -= OnInteractableObjectDestroyed;

                PlayerStateMachine.ReusableData.InteractableObject = null;
            }
        }

        public override void OnAnimationExitEvent()
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerChasingState);
        }

        protected override void OnInteractableCheck()
        {
        }

        public override void Exit()
        {
            base.Exit();

            PlayerStateMachine.AnimationChanger.StopAnimation(PlayerStateMachine.AnimationData.AttackParameterHash);

            if (PlayerStateMachine.ReusableData.InteractableObject != null)
                PlayerStateMachine.ReusableData.InteractableObject.OnDestroyed -= OnInteractableObjectDestroyed;
        }
    }
}
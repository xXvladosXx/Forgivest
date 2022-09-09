using Characters.Player.StateMachines.Movement.States.Grounded;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Movement.States.Grounded
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private bool _shouldKeepRotating;

        public override void Enter()
        {
            PlayerStateMachine.ReusableData.MovementSpeedModifier = GroundedData.DashData.SpeedModifier;
            PlayerStateMachine.ReusableData.MaxSmoothModifier = GroundedData.RunData.SmoothInputSpeed;

            base.Enter();

            StartAnimation(PlayerStateMachine.Player.AnimationData.DashParameterHash);

            PlayerStateMachine.ReusableData.SmoothModifier =
                PlayerStateMachine.ReusableData.MaxSmoothModifier;
            
            _shouldKeepRotating = PlayerStateMachine.ReusableData.MovementInput != Vector2.zero;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerStateMachine.Player.AnimationData.DashParameterHash);
        }

        public override void OnAnimationEnterEvent()
        {
            base.OnAnimationEnterEvent();
            Dash();
        }

        public override void OnAnimationTransitionEvent()
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

           // PlayerStateMachine.Player.Input.PlayerActions.Movement.performed += OnMovementPerformed;
           // PlayerStateMachine.Player.Input.PlayerActions.Dash.performed -= OnDashStarted;

        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            //PlayerStateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
        }


        private void Dash()
        {
            Vector3 dashDirection = PlayerStateMachine.Player.transform.forward;

            dashDirection.y = 0f;


            PlayerStateMachine.Player.Rigidbody.velocity = dashDirection * GetMovementSpeed();
        }

        public PlayerDashingState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }
    }
}
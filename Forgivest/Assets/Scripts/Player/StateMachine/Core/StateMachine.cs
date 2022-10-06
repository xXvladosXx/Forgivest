using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using UnityEngine;

namespace StateMachine
{
    public abstract class StateMachine 
    {
        private IState _currentState;

        public void ChangeState(IState newState)
        {
            _currentState?.Exit();

            _currentState = newState;

            _currentState.Enter();
        }

        public void Update()
        {
            Debug.Log(_currentState);
            _currentState?.Update();
        }

        public void PhysicsUpdate()
        {
            _currentState?.FixedUpdate();
        }

        public void OnAnimationEnterEvent()
        {
            _currentState?.OnAnimationEnterEvent();
        }

        public void OnAnimationExitEvent()
        {
            _currentState?.OnAnimationExitEvent();
        }

        public void OnAnimationTransitionEvent()
        {
            _currentState?.OnAnimationTransitionEvent();
        }
    }
}
using AttackSystem.Core;
using Zenject;

namespace GameCore.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly DiContainer _diContainer;

        public GameLoopState(GameStateMachine gameStateMachine,
            DiContainer diContainer)
        {
            _gameStateMachine = gameStateMachine;
            _diContainer = diContainer;
        }
        
        public void Enter()
        {
            _diContainer.Resolve<DamageHandler>().OnDied += OnPlayerDied;
        }
        public void Exit()
        {
            _diContainer.Resolve<DamageHandler>().OnDied -= OnPlayerDied;
        }

        private void OnPlayerDied(AttackData attackData)
        {   
            _gameStateMachine.Enter<GameEndState>();
        }
    }
}
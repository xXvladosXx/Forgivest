using AttackSystem.Core;
using GameCore.Factory;
using Zenject;

namespace GameCore.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IGameFactory _gameFactory;
        private readonly IPlayerObserver _playerObserver;

        public GameLoopState(GameStateMachine gameStateMachine,
            IGameFactory gameFactory)
        {
            _gameStateMachine = gameStateMachine;
            _gameFactory = gameFactory;
        }
        
        public void Enter()
        {
            _gameFactory.PlayerObserver.DamageHandler.OnDied += OnPlayerDied;
        }

        public void Exit()
        {
            _gameFactory.PlayerObserver.DamageHandler.OnDied -= OnPlayerDied;
        }

        private void OnPlayerDied(AttackData attackData)
        {   
            _gameStateMachine.Enter<GameEndState>();
        }
    }
}
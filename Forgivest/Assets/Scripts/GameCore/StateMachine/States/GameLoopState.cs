using AttackSystem.Core;
using GameCore.Crutches;
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
            _gameFactory.UIObserver.GameplayMenu.OnMainMenuButtonClicked += OnMainMenuButtonClicked;
        }

        public void Exit()
        {
            _gameFactory.PlayerObserver.DamageHandler.OnDied -= OnPlayerDied;    
            _gameFactory.UIObserver.GameplayMenu.OnMainMenuButtonClicked -= OnMainMenuButtonClicked;
        }

        private void OnPlayerDied(AttackData attackData)
        {   
            _gameStateMachine.Enter<GameEndState>();
        }

        private void OnMainMenuButtonClicked()
        {
            _gameStateMachine.Enter<LoadMainMenuState, string>("MainMenu");
        }
    }
}
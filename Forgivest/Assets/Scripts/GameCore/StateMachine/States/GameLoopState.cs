using AttackSystem.Core;
using GameCore.Crutches;
using GameCore.Factory;
using SoundSystem;
using Zenject;

namespace GameCore.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IGameFactory _gameFactory;
        private readonly SoundManger _soundManger;
        private readonly IPlayerObserver _playerObserver;

        public GameLoopState(GameStateMachine gameStateMachine,
            IGameFactory gameFactory, SoundManger soundManger)
        {
            _gameStateMachine = gameStateMachine;
            _gameFactory = gameFactory;
            _soundManger = soundManger;
        }
        
        public void Enter()
        {
            _gameFactory.PlayerObserver.DamageHandler.OnDied += OnPlayerDied;
            _gameFactory.UIObserver.GameplayMenu.OnMainMenuButtonClicked += OnMainMenuButtonClicked;
            
            _soundManger.PlayMusicSound(_soundManger.GameMusicClips[4]);
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
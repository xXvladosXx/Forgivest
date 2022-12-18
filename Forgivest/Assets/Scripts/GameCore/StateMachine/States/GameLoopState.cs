using AttackSystem.Core;
using GameCore.Crutches;
using GameCore.Factory;
using SoundSystem;
using UI.Core;
using UI.Menu;
using UnityEngine;
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
        private LoadMenu _loadMenu;


        public void Enter()
        { 
            var findObjectOfType = Object.FindObjectOfType<PanelSwitcher>();

            if(findObjectOfType != null)
            {
                _loadMenu = findObjectOfType.LoadMenu;
            }

            _gameFactory.PlayerObserver.DamageHandler.OnDied += OnPlayerDied;
            _gameFactory.UIObserver.GameplayMenu.OnMainMenuButtonClicked += OnMainMenuButtonClicked;
            _loadMenu.OnLoadClicked += LoadSave;

            _soundManger.PlayMusicSound(_soundManger.GameMusicClips[4]);
        }


        public void Exit()
        {
            _gameFactory.PlayerObserver.DamageHandler.OnDied -= OnPlayerDied;    
            _gameFactory.UIObserver.GameplayMenu.OnMainMenuButtonClicked -= OnMainMenuButtonClicked;
            _loadMenu.OnLoadClicked -= LoadSave;
        }

        private void OnPlayerDied(AttackData attackData)
        {   
            _gameStateMachine.Enter<GameEndState>();
        }
        
        private void LoadSave(string saveFile)
        {
            _gameStateMachine.Enter<LoadExistingGameState, string>(saveFile);
        }

        private void OnMainMenuButtonClicked()
        {
            _gameStateMachine.Enter<LoadMainMenuState, string>("MainMenu");
        }
    }
}
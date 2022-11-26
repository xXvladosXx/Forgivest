using GameCore.Factory;
using UI.Menu;
using UI.Menu.Core;
using UnityEngine;
using Zenject;

namespace GameCore.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly MenuSwitcher _mainMenuSwitcher;
        private readonly IGameFactory _gameFactory;
        
        private StartMenu _startMenu;
        private LoadMenu _loadMenu;
        
        public BootstrapState(GameStateMachine gameStateMachine,
            MenuSwitcher mainMenuSwitcher, DiContainer diContainer)
        {
            _gameStateMachine = gameStateMachine;
            _mainMenuSwitcher = mainMenuSwitcher;
        }

        public void Enter()
        {
            _startMenu = _mainMenuSwitcher.Find<StartMenu>() as StartMenu;
            _loadMenu = _mainMenuSwitcher.Find<LoadMenu>() as LoadMenu;

            _startMenu.OnStartClicked += LoadStartScene;
            _loadMenu.OnLoadClicked += LoadScene;
        }

        public void Exit()
        {
            
        }

        private void LoadStartScene()
        {
            _startMenu.OnStartClicked -= LoadScene;
            EnterStartLevel();
        }

        private void LoadScene()
        {
            _loadMenu.OnLoadClicked -= LoadStartScene;
            EnterLoadLevel();
        }

        private void EnterStartLevel()
        {
            _gameStateMachine.Enter<StartNewGameState, string>("NewGame");
        }

        private void EnterLoadLevel()
        {
            _gameStateMachine.Enter<LoadExistingGameState, string>("NoGame");
        }
    }
}
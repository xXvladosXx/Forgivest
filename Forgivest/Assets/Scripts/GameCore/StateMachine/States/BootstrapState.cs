using GameCore.Factory;
using GameCore.SaveSystem.Reader;
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
        private MainMenu _mainMenu;

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
            _mainMenu = _mainMenuSwitcher.Find<MainMenu>() as MainMenu;

            _startMenu.OnStartClicked += LoadStartGame;
            _loadMenu.OnLoadClicked += LoadExistingGame;
            _mainMenu.OnContinueClick += LoadLastGame;
        }

        public void Exit()
        {
            
        }

        private void LoadStartGame(string save)
        {
            _startMenu.OnStartClicked -= LoadStartGame;
            EnterStartLevel(save);
        }

        private void LoadExistingGame(string save)
        {
            _loadMenu.OnLoadClicked -= LoadExistingGame;
            EnterLoadLevel(save);
        }

        private void LoadLastGame()
        {
            _mainMenu.OnContinueClick -= LoadLastGame;
            EnterLoadLevel(FileManager.GetLastSave);
        }

        private void EnterStartLevel(string save)
        {
            _gameStateMachine.Enter<StartNewGameState, string>(save);
        }

        private void EnterLoadLevel(string save)
        {
            _gameStateMachine.Enter<LoadExistingGameState, string>(save);
        }
    }
}
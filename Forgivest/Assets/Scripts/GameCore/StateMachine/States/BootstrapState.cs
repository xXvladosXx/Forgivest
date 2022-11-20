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
        private readonly DiContainer _diContainer;
        private readonly IGameFactory _gameFactory;
        
        private StartMenu _startMenu;
        
        public BootstrapState(GameStateMachine gameStateMachine,
            MenuSwitcher mainMenuSwitcher, DiContainer diContainer)
        {
            _gameStateMachine = gameStateMachine;
            _mainMenuSwitcher = mainMenuSwitcher;
            _diContainer = diContainer;
        }

        public void Enter()
        {
            _startMenu = _mainMenuSwitcher.Find<StartMenu>() as StartMenu;
            
            if (_startMenu != null) 
                _startMenu.OnStartClicked += LoadScene;
        }

        public void Exit()
        {
            
        }

        private void LoadScene()
        {
            _startMenu.OnStartClicked -= LoadScene;
            EnterLoadLevel();
        }

        private void EnterLoadLevel()
        {
            _gameStateMachine.Enter<LoadProgressState>();
        }
    }
}
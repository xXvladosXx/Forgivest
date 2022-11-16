using UI.Menu;
using UI.Menu.Core;
using UnityEngine;

namespace GameCore.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly MenuSwitcher _mainMenuSwitcher;
        private readonly Canvas _mainMenuCanvas;

        private StartMenu _startMenu;
        
        private const string SAMPLE_SCENE = "MainMenu";

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            MenuSwitcher mainMenuSwitcher, Canvas mainMenuCanvas)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _mainMenuSwitcher = mainMenuSwitcher;
            _mainMenuCanvas = mainMenuCanvas;
        }

        public void Enter()
        {
            _startMenu = _mainMenuSwitcher.Find<StartMenu>() as StartMenu;
            
            if (_startMenu != null) 
                _startMenu.OnStartClicked += LoadScene;
        }

        private void LoadScene()
        {
            _startMenu.OnStartClicked -= LoadScene;
            EnterLoadLevel();
        }

        private void EnterLoadLevel()
        {
            _gameStateMachine.Enter<LoadLevelState, string>("SampleScene");
        }

        public void Exit()
        {
            
        }
    }
}
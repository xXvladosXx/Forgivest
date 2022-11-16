using UI.Menu;
using UI.Menu.Core;

namespace GameCore.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly MenuSwitcher _mainMenuSwitcher;

        private const string SAMPLE_SCENE = "MainMenu";

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, MenuSwitcher mainMenuSwitcherSwitcher)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _mainMenuSwitcher = mainMenuSwitcherSwitcher;
        }

        public void Enter()
        {
            _sceneLoader.Load(SAMPLE_SCENE, EnterLoadLevel);

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
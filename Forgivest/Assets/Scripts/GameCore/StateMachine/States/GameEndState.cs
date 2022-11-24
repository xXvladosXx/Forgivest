using UI.Loading;
using UI.Utils;
using Zenject;

namespace GameCore.StateMachine.States
{
    public class GameEndState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly PersistentCanvas _persistentCanvas;

        public GameEndState(GameStateMachine gameStateMachine, PersistentCanvas persistentCanvas)
        {
            _gameStateMachine = gameStateMachine;
            _persistentCanvas = persistentCanvas;
        }

        public void Enter()
        {
            _persistentCanvas.gameObject.SetActive(true);
            _persistentCanvas.ShowDeathScreen();
        }

        public void Exit()
        {
            
        }
    }
}
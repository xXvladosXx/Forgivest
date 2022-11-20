using Zenject;

namespace GameCore.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;

        public GameLoopState(GameStateMachine gameStateMachine, DiContainer diContainer)
        {
            _gameStateMachine = gameStateMachine;
        }
        public void Exit()
        {
            
        }

        public void Enter()
        {
        }
    }
}
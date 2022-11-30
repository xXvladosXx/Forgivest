using GameCore.Crutches;
using GameCore.Factory;
using SoundSystem;

namespace GameCore.StateMachine.States
{
    public class GameBattleState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IGameFactory _gameFactory;
        private readonly SoundManger _soundManger;
        private readonly IPlayerObserver _playerObserver;

        public GameBattleState(GameStateMachine gameStateMachine,
            IGameFactory gameFactory, SoundManger soundManger)
        {
            _gameStateMachine = gameStateMachine;
            _gameFactory = gameFactory;
            _soundManger = soundManger;
        }
        
        public void Enter()
        {
        }

        public void Exit()
        {
            
        }
    }
}
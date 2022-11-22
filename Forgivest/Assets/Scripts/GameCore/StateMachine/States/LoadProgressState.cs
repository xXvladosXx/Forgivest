using GameCore.Data;
using GameCore.Data.SaveLoad;

namespace GameCore.StateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadService _saveLoadService;

        private const string GAMEPLAY = "Gameplay";
        private const string ENVIRONMENT = "FAE_Demo1";

        public LoadProgressState(GameStateMachine gameStateMachine, 
            IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService; 
        }

        public void Enter()
        {
            LoadProgress();
            _gameStateMachine.Enter<LoadLevelState, string>(
                _persistentProgressService.PlayerProgress
                .WorldData.PositionOnLevel.Level);
        }

        public void Exit()
        {
            
        }

        private void LoadProgress()
        {
            _persistentProgressService.PlayerProgress = _saveLoadService.Load() ?? NewProgress();
        }

        private PlayerProgress NewProgress() => new PlayerProgress(ENVIRONMENT);
    }
}
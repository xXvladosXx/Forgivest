using GameCore.Data;
using GameCore.Data.SaveLoad;

namespace GameCore.StateMachine.States
{
    public abstract class LoadGameState : ILoadState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        protected readonly IPersistentProgressService PersistentProgressService;
        protected readonly ISaveLoadService SaveLoadService;
        
        public LoadGameState(GameStateMachine gameStateMachine, 
            IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            PersistentProgressService = persistentProgressService;
            SaveLoadService = saveLoadService;
        }
        
        public void Enter(string saveFile)
        {
            StartGame(saveFile);
            _gameStateMachine.Enter<LoadLevelState, string>(
                PersistentProgressService.PlayerProgresses[saveFile]
                    .WorldData.PositionOnLevel.Level);
        }

        public void Exit()
        {
            
        }

        protected abstract void StartGame(string saveFile);
    }
}
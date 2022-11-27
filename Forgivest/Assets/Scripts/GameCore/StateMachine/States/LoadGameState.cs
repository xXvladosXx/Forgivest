using GameCore.SaveSystem.Data;
using GameCore.SaveSystem.SaveLoad;

namespace GameCore.StateMachine.States
{
    public abstract class LoadGameState : ILoadState<string>
    {
        protected readonly GameStateMachine _gameStateMachine;
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
        
        public virtual void Enter(string saveFile)
        {
            StartGame(saveFile);
            _gameStateMachine.Enter<LoadLevelState, string>(
                PersistentProgressService.PlayerProgress
                    .WorldData.PositionOnLevel.Level);
        }

        public void Exit()
        {
            
        }

        protected abstract void StartGame(string saveFile);
    }
}
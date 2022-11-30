using GameCore.SaveSystem.Data;
using GameCore.SaveSystem.SaveLoad;
using SoundSystem;

namespace GameCore.StateMachine.States
{
    public abstract class LoadGameState : ILoadState<string>
    {
        protected readonly GameStateMachine GameStateMachine;
        protected readonly IPersistentProgressService PersistentProgressService;
        protected readonly ISaveLoadService SaveLoadService;

        private readonly SoundManger _soundManger;

        public LoadGameState(GameStateMachine gameStateMachine, 
            IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService, SoundManger soundManger)
        {
            GameStateMachine = gameStateMachine;
            PersistentProgressService = persistentProgressService;
            SaveLoadService = saveLoadService;
            _soundManger = soundManger;
        }
        
        public virtual void Enter(string saveFile)
        {
            _soundManger.StopPlayingMusic();

            StartGame(saveFile);
            GameStateMachine.Enter<LoadLevelState, string>(
                PersistentProgressService.PlayerProgress
                    .WorldData.PositionOnLevel.Level);
        }

        public void Exit()
        {
            
        }

        protected abstract void StartGame(string saveFile);
    }
}
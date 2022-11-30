using GameCore.SaveSystem.Data;
using GameCore.SaveSystem.SaveLoad;
using SoundSystem;

namespace GameCore.StateMachine.States
{
    public class StartNewGameState : LoadGameState
    {
        private const string ENVIRONMENT = "MainScene";

        public StartNewGameState(GameStateMachine gameStateMachine, 
            IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService, SoundManger soundManger) 
            : base(gameStateMachine, persistentProgressService, saveLoadService, soundManger)
        {
        }

        protected override void StartGame(string saveFile)
        {
            PersistentProgressService.PlayerProgress = NewProgress();
        }
        
        private PlayerProgress NewProgress() => new PlayerProgress(ENVIRONMENT);
    }
}
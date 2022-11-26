using GameCore.Data;
using GameCore.Data.SaveLoad;

namespace GameCore.StateMachine.States
{
    public class StartNewGameState : LoadGameState
    {
        private const string ENVIRONMENT = "FAE_Demo1";

        public StartNewGameState(GameStateMachine gameStateMachine, 
            IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService) 
            : base(gameStateMachine, persistentProgressService, saveLoadService)
        {
        }

        protected override void StartGame(string saveFile)
        {
            PersistentProgressService.PlayerProgress = NewProgress();
        }
        
        private PlayerProgress NewProgress() => new PlayerProgress(ENVIRONMENT);
    }
}
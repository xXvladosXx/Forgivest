using GameCore.SaveSystem.Data;
using GameCore.SaveSystem.SaveLoad;

namespace GameCore.StateMachine.States
{
    public class LoadExistingGameState : LoadGameState
    {
        public LoadExistingGameState(GameStateMachine gameStateMachine,
            IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService)
            : base(gameStateMachine, persistentProgressService, saveLoadService)
        {
        }

        protected override void StartGame(string saveFile)
        {
            LoadProgress(saveFile);
        }

        private void LoadProgress(string saveFile)
        {
            PersistentProgressService.PlayerProgress = SaveLoadService.Load(saveFile);
        }
    }
}
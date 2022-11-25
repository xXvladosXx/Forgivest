using GameCore.Data;
using GameCore.Data.SaveLoad;

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
            PersistentProgressService.PlayerProgresses[saveFile] = SaveLoadService.Load(saveFile);
        }
    }
}
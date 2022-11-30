using GameCore.SaveSystem.Data;
using GameCore.SaveSystem.SaveLoad;
using SoundSystem;

namespace GameCore.StateMachine.States
{
    public class LoadExistingGameState : LoadGameState
    {
        public LoadExistingGameState(GameStateMachine gameStateMachine,
            IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService, SoundManger soundManger)
            : base(gameStateMachine, persistentProgressService, saveLoadService, soundManger)
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
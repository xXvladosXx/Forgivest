using GameCore.SaveSystem.Data;
using GameCore.SaveSystem.SaveLoad;
using SoundSystem;

namespace GameCore.StateMachine.States
{
    public class LoadMainMenuState : LoadGameState
    {
        public LoadMainMenuState(GameStateMachine gameStateMachine,
            IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService, SoundManger soundManger) : 
            base(gameStateMachine, persistentProgressService, saveLoadService, soundManger)
        {
        }

        public override void Enter(string saveFile)
        {
            GameStateMachine.Enter<LoadLevelState, string>("MainMenu");
        }

        protected override void StartGame(string saveFile)
        {
            
        }
    }
}
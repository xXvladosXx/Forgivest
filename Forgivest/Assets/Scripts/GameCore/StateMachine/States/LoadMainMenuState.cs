using GameCore.SaveSystem.Data;
using GameCore.SaveSystem.SaveLoad;

namespace GameCore.StateMachine.States
{
    public class LoadMainMenuState : LoadGameState
    {
        public LoadMainMenuState(GameStateMachine gameStateMachine,
            IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService) : 
            base(gameStateMachine, persistentProgressService, saveLoadService)
        {
        }

        public override void Enter(string saveFile)
        {
            _gameStateMachine.Enter<LoadLevelState, string>("MainMenu");
        }

        protected override void StartGame(string saveFile)
        {
            
        }
    }
}
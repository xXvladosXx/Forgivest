using System.Collections;
using UI.Loading;
using UI.Utils;
using UnityEngine;
using Utilities;
using Zenject;

namespace GameCore.StateMachine.States
{
    public class GameEndState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly PersistentCanvas _persistentCanvas;

        public GameEndState(GameStateMachine gameStateMachine, PersistentCanvas persistentCanvas)
        {
            _gameStateMachine = gameStateMachine;
            _persistentCanvas = persistentCanvas;
        }

        public void Enter()
        {
            _persistentCanvas.gameObject.SetActive(true);
            _persistentCanvas.ShowDeathScreen();

            Coroutines.StartRoutine(BackToMainMenu());
        }

        private IEnumerator BackToMainMenu()
        {
            yield return new WaitForSeconds(4f);
            _gameStateMachine.Enter<LoadMainMenuState, string>("MainMenu");
        }

        public void Exit()
        {
            _persistentCanvas.HideDeathScreen();
        }
    }
}
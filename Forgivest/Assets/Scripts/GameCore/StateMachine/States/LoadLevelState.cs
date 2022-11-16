using UI.Loading;
using UnityEngine;

namespace GameCore.StateMachine.States
{
    public class LoadLevelState : ILoadState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingScreen _loadingScreen;

        private const string PLAYER = "Player";
        private const string PLAYER_INITIAL_POINT = "PlayerInitialPoint";

        public LoadLevelState(GameStateMachine gameStateMachine,
            SceneLoader sceneLoader, LoadingScreen loadingScreen)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
        }

        public void Enter(string sceneName)
        {
            _loadingScreen.ShowLoadingScreen();
            _loadingScreen.ShowLoadingBar();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _loadingScreen.HideLoadingScreen();
            _loadingScreen.OnStartGame -= StartGame;
        }
        
        private void OnLoaded()
        {
            var initialPoint = GameObject.FindWithTag(PLAYER_INITIAL_POINT);
            var player = GameObject.FindWithTag(PLAYER);

            player.transform.position = initialPoint.transform.position;
            _loadingScreen.LoadProgress(1);

            _loadingScreen.OnStartGame += StartGame;
        }

        private void StartGame()
        {
            _gameStateMachine.Enter<GameLoopState>();
        }
    }
}
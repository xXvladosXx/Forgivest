using System;
using UI.Loading;
using UnityEngine;
using Utilities;

namespace GameCore.StateMachine.States
{
    public class LoadLevelState : ILoadState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingScreen _loadingScreen;

        private const string PLAYER = "Player";
        private const string PLAYER_INITIAL_POINT = "PlayerInitialPoint";

        public event Action OnGameStarted;
        
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
            _loadingScreen.OnStartGame -= OnGameStarted;
        }
        
        private void OnLoaded()
        {
            var initialPoint = GameObject.FindWithTag(PLAYER_INITIAL_POINT);
            var player = GameObject.FindWithTag(PLAYER);
            player.GetComponent<PlayerInputProvider>().enabled = false;

            player.transform.position = initialPoint.transform.position;
            _loadingScreen.LoadProgress(1);

            OnGameStarted += () =>
            {
                player.GetComponent<PlayerInputProvider>().enabled = true;
                _gameStateMachine.Enter<GameLoopState>();
            };
            
            _loadingScreen.OnStartGame += OnGameStarted;
        }
    }
}
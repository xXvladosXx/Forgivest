using System;
using GameCore.Data;
using GameCore.Factory;
using UI.Loading;
using UnityEngine;
using Utilities;
using Zenject;

namespace GameCore.StateMachine.States
{
    public class LoadLevelState : ILoadState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingScreen _loadingScreen;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _persistentProgressService;

        private const string PLAYER_INITIAL_POINT = "PlayerInitialPoint";

        public event Action OnGameStarted;
        
        public LoadLevelState(GameStateMachine gameStateMachine,
            SceneLoader sceneLoader, LoadingScreen loadingScreen,
            IGameFactory gameFactory, IPersistentProgressService persistentProgressService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
            _gameFactory = gameFactory;
            _persistentProgressService = persistentProgressService;
        }

        public void Enter(string sceneName)
        {
            _loadingScreen.ShowLoadingScreen();
            _loadingScreen.ShowLoadingBar();
            _gameFactory.CleanUp();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _loadingScreen.HideLoadingScreen();
            _loadingScreen.OnStartGame -= OnGameStarted;
        }
        
        private void OnLoaded()
        {
            _loadingScreen.LoadProgress(1);
            InformProgressReaders();

            OnGameStarted += () =>
            {
                _gameStateMachine.Enter<GameLoopState>();
            };
            
            _loadingScreen.OnStartGame += OnGameStarted;
        }

        private void InformProgressReaders()
        {
            foreach (var progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_persistentProgressService.PlayerProgress);
            }
        }
    }
}
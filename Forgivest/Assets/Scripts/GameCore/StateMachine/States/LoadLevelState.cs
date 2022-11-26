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
        private readonly PersistentCanvas _persistentCanvas;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _persistentProgressService;

        public event Action OnGameStarted;
        
        public LoadLevelState(GameStateMachine gameStateMachine,
            SceneLoader sceneLoader, PersistentCanvas persistentCanvas,
            IGameFactory gameFactory, IPersistentProgressService persistentProgressService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _persistentCanvas = persistentCanvas;
            _gameFactory = gameFactory;
            _persistentProgressService = persistentProgressService;
        }

        public void Enter(string saveFile)
        {
            _persistentCanvas.ShowLoadingScreen();
            _persistentCanvas.ShowLoadingBar();
            _gameFactory.CleanUp();
            _sceneLoader.Load(_persistentProgressService.PlayerProgress.Scene, OnLoaded(saveFile));
        }

        public void Exit()
        {
            _persistentCanvas.HideLoadingScreen();
            _persistentCanvas.OnStartGame -= OnGameStarted;
        }
        
        private string OnLoaded(string saveFile)
        {
            _persistentCanvas.LoadProgress(1);
            InformProgressReaders(saveFile);

            OnGameStarted += () =>
            {
                _gameStateMachine.Enter<GameLoopState>();
            };
            
            _persistentCanvas.OnStartGame += OnGameStarted;
            return null;
        }

        private void InformProgressReaders(string saveFile)
        {
            foreach (var progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_persistentProgressService.PlayerProgress);
            }
        }
    }
}
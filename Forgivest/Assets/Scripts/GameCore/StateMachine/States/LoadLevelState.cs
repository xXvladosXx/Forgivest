﻿using System;
using GameCore.Factory;
using GameCore.SaveSystem.Data;
using SoundSystem;
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
        private readonly SoundManger _soundManger;

        public event Action OnGameStarted;
        
        public LoadLevelState(GameStateMachine gameStateMachine,
            SceneLoader sceneLoader, PersistentCanvas persistentCanvas,
            IGameFactory gameFactory, IPersistentProgressService persistentProgressService, 
            SoundManger soundManger)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _persistentCanvas = persistentCanvas;
            _gameFactory = gameFactory;
            _persistentProgressService = persistentProgressService;
            _soundManger = soundManger;
        }

        public void Enter(string saveFile)
        {
            _persistentCanvas.ShowLoadingScreen();
            _persistentCanvas.ShowLoadingBar();
            _gameFactory.CleanUp();
            _sceneLoader.Load(saveFile, OnLoaded);
        }

        public void Exit()
        {
            _persistentCanvas.HideLoadingScreen();
            _persistentCanvas.OnStartGame -= OnGameStarted;
        }
        
        private void OnLoaded()
        {
            _persistentCanvas.LoadProgress(1);
            InformProgressReaders();

            OnGameStarted += () =>
            {
                _gameStateMachine.Enter<GameLoopState>();
            };
            
            _persistentCanvas.OnStartGame += OnGameStarted;
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
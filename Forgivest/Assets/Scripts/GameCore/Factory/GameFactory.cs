﻿using System.Collections.Generic;
using GameCore.AssetManagement;
using GameCore.Data;
using GameCore.StateMachine;
using UnityEngine;

namespace GameCore.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        
        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        public IGameObserver PlayerEntity { get; }
        public GameFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            PlayerEntity = new GameObserver();
        }

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }
        
        public GameObject CreatePlayer(GameObject at) => 
            InstantiateRegistered(AssetPath.PLAYER_PATH, at.transform.position);

        public void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
            {
                ProgressWriters.Add(progressWriter);
            }
            
            ProgressReaders.Add(progressReader);
        }

        private GameObject InstantiateRegistered(string path, Vector3 position)
        {
            var gameObject = _assetProvider.Instantiate(path, position);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string path)
        {
            var gameObject = _assetProvider.Instantiate(path);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject player)
        {
            foreach (var progressReader in player.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }
    }
}
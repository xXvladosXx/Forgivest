using System.Collections.Generic;
using GameCore.Data.Types;
using GameCore.Factory;
using ModestTree;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.Data.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;
        
        private const string PROGRESS = "Progress";

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
        }
        
        public void SaveProgress(string saveFile)
        {
            var capturedStates = FileManager.LoadFromBinaryFile(saveFile);

            if (capturedStates == null)
            {
                capturedStates = new PlayerProgress(SceneManager.GetActiveScene().name);
                FileManager.SaveToBinaryFile(saveFile, capturedStates);
                return;
            }
            
            foreach (var progressWriter in _gameFactory.ProgressWriters)
            {
                progressWriter.UpdateProgress(capturedStates);
            }
            
            FileManager.SaveToBinaryFile(saveFile, capturedStates);
        }


        public PlayerProgress Load(string saveFile)
        {
            var restoredStates = FileManager.LoadFromBinaryFile(saveFile);
            return restoredStates;
        }
    }
}
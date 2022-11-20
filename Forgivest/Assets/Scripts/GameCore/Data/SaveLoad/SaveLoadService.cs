using GameCore.Data.Types;
using GameCore.Factory;
using UnityEngine;

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
        
        public void SaveProgress()
        {
            foreach (var progressWriter in _gameFactory.ProgressWriters)
            {
                progressWriter.UpdateProgress(_progressService.PlayerProgress);
            }
            
            PlayerPrefs.SetString(PROGRESS, _progressService.PlayerProgress.ToJson());
        }

        public PlayerProgress Load() => 
            PlayerPrefs.GetString(PROGRESS)
                ?.ToDeserializedObject<PlayerProgress>();
    }
}
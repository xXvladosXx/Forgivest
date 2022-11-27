using GameCore.Factory;
using GameCore.SaveSystem.Data;
using GameCore.SaveSystem.Reader;
using UnityEngine.SceneManagement;

namespace GameCore.SaveSystem.SaveLoad
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

            capturedStates.Scene = SceneManager.GetActiveScene().name;
            
            FileManager.SaveToBinaryFile(saveFile, capturedStates);
        }


        public PlayerProgress Load(string saveFile)
        {
            var restoredStates = FileManager.LoadFromBinaryFile(saveFile);
            return restoredStates;
        }
    }
}
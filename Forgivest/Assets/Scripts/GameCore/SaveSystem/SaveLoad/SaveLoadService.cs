using GameCore.Factory;
using GameCore.SaveSystem.Data;
using GameCore.SaveSystem.Reader;
using SoundSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.SaveSystem.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;
        
        private const string EFFECTS_VOLUME = "EffectsVolume";
        private const string MUSIC_VOLUME = "MusicVolume";
    
        public SaveLoadService(IPersistentProgressService progressService, 
            IGameFactory gameFactory)
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
        
        public void SaveAudioSettings(AudioSettingsData audioSettingsData)
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME, audioSettingsData.MusicVolume);
            PlayerPrefs.SetFloat(EFFECTS_VOLUME, audioSettingsData.EffectsVolume);
        }
        
        public AudioSettingsData LoadAudioSettings()
        {
            var settingsSaveData = new AudioSettingsData(
                PlayerPrefs.GetFloat(MUSIC_VOLUME, 0),
                PlayerPrefs.GetFloat(EFFECTS_VOLUME, 0));

            return settingsSaveData;
        }
    }
}
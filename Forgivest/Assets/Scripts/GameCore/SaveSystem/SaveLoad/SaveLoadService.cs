using GameCore.Factory;
using GameCore.SaveSystem.Data;
using GameCore.SaveSystem.Reader;
using SoundSystem;
using UI.Menu;
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
        private const string RESOLUTION = "Resolution";
        private const string IS_FULLSCREEN = "Fullscreen";
        private const string GRAPHICS = "Graphics";
    
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

        public void SaveGraphicsSettings(int resolution, int isFullscreen, int graphics)
        {
            PlayerPrefs.SetInt(RESOLUTION, resolution);
            PlayerPrefs.SetInt(IS_FULLSCREEN, isFullscreen);
            PlayerPrefs.SetInt(GRAPHICS, graphics);
        }
        
        public void SaveAudioSettings(float musicVolume, float effectsVolume)
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME, musicVolume);
            PlayerPrefs.SetFloat(EFFECTS_VOLUME, effectsVolume);
        }
        
        public SettingsData LoadSettings()
        {
            var settingsSaveData = new SettingsData(
                PlayerPrefs.GetFloat(MUSIC_VOLUME, 0),
                PlayerPrefs.GetFloat(EFFECTS_VOLUME, 0),
                PlayerPrefs.GetInt(GRAPHICS, 0),
                PlayerPrefs.GetInt(IS_FULLSCREEN, 0),
                PlayerPrefs.GetInt(RESOLUTION,0));

            return settingsSaveData;
        }
    }
}
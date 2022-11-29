using System;
using GameCore.SaveSystem.Reader;
using GameCore.SaveSystem.SaveLoad;
using SoundSystem;
using UI.Menu;
using Zenject;

namespace Controllers
{
    public class MainMenuController : IInitializable, IDisposable
    {
        private readonly StartMenu _startMenu;
        private readonly LoadMenu _loadMenu;
        private readonly SettingsMenu _settingsMenu;
        private readonly SoundMenu _soundMenu;
        private readonly ISaveLoadService _saveLoadService;
        private readonly SoundManger _soundManger;

        public MainMenuController(StartMenu startMenu,
            LoadMenu loadMenu, SettingsMenu settingsMenu,
            SoundMenu soundMenu, ISaveLoadService saveLoadService,
            SoundManger soundManger)
        {
            _startMenu = startMenu;
            _loadMenu = loadMenu;
            _settingsMenu = settingsMenu;
            _soundMenu = soundMenu;
            _saveLoadService = saveLoadService;
            _soundManger = soundManger;
        }

        public void Initialize()
        {
            _loadMenu.Initialize(FileManager.SavesList());

            var audioSettings = _saveLoadService.LoadAudioSettings();
            
            _soundMenu.LoadAudioSettings(audioSettings);
            _soundManger.SetMusicSound(audioSettings.MusicVolume);
            _soundManger.SetEffectsSound(audioSettings.EffectsVolume);
            
            _soundMenu.OnAudioSettingsChanged += SaveSettings;
            
            _soundManger.PlayMusicSound(_soundManger.GameMusicClips[0]);
        }

        public void Dispose()
        {
            _soundMenu.OnAudioSettingsChanged -= SaveSettings;
        }

        private void SaveSettings(float music, float effects)
        {
            _saveLoadService.SaveAudioSettings(new AudioSettingsData(music, effects));
            _soundManger.SetMusicSound(music);
            _soundManger.SetEffectsSound(effects);
        }
    }
}
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
        private readonly GraphicsMenu _graphicsMenu;
        private readonly ISaveLoadService _saveLoadService;
        private readonly SoundManger _soundManger;

        public MainMenuController(StartMenu startMenu,
            LoadMenu loadMenu, SettingsMenu settingsMenu,
            SoundMenu soundMenu,GraphicsMenu graphicsMenu, 
            ISaveLoadService saveLoadService,
            SoundManger soundManger)
        {
            _startMenu = startMenu;
            _loadMenu = loadMenu;
            _settingsMenu = settingsMenu;
            _soundMenu = soundMenu;
            _graphicsMenu = graphicsMenu;
            _saveLoadService = saveLoadService;
            _soundManger = soundManger;
        }

        public void Initialize()
        {
            _loadMenu.Initialize(FileManager.SavesList());

            var settings = _saveLoadService.LoadSettings();
            
            _soundMenu.LoadAudioSettings(settings);
            _soundManger.SetMusicSound(settings.MusicVolume);
            _soundManger.SetEffectsSound(settings.EffectsVolume);

            _graphicsMenu.Init();
            _graphicsMenu.FindResolution(settings.ResolutionIndex);
            _graphicsMenu.SetFullscreen(Convert.ToBoolean(settings.IsFullScreen));
            _graphicsMenu.SetQualityLevelDropdown(settings.QualityLevel);
            
            _soundMenu.OnAudioSettingsChanged += SaveAudioSettings;
            _graphicsMenu.OnGraphicsSettingsChanged += SaveGraphicsSettings;
            
            _soundManger.PlayMusicSound(_soundManger.GameMusicClips[0]);
        }

        private void SaveSettings(float music, float effects)
        {
            _saveLoadService.SaveAudioSettings(music, effects);
        }

        private void SaveGraphicsSettings(int resolution, int isFullscreen, int graphics)
        {
            _saveLoadService.SaveGraphicsSettings(resolution, isFullscreen, graphics);
        }

        public void Dispose()
        {
            _soundMenu.OnAudioSettingsChanged -= SaveAudioSettings;
            _graphicsMenu.OnGraphicsSettingsChanged -= SaveGraphicsSettings;
        }
    }
}
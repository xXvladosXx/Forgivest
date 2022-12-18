using System;
using System.Linq;
using GameCore.Factory;
using GameCore.SaveSystem.Reader;
using GameCore.SaveSystem.SaveLoad;
using Installers;
using Player;
using SoundSystem;
using UI.Core;
using UI.HUD;
using UI.Menu;
using UI.Menu.Core;
using UI.Skill;
using UnityEngine.InputSystem;
using Utilities;
using Zenject;
using InventoryPanel = UI.Inventory.Core.InventoryPanel;

namespace Controllers
{
    public class PlayerPanelController : IInitializable, IDisposable
    {
        private readonly PanelSwitcher _panelSwitcher;
        private readonly IGameFactory _gameFactory;
        private readonly PlayerInputProvider _playerInputProvider;
        private readonly GameplayMenu _gameplayMenu;
        private readonly LoadMenu _loadMenu;
        private readonly SaveMenu _saveMenu;
        private readonly SettingsMenu _settingsMenu;
        private readonly SoundMenu _soundMenu;
        private readonly IEnemyRadiusChecker _enemyRadiusChecker;
        private readonly StaticInventoryPanel _staticInventoryPanel;
        private readonly SoundManger _soundManger;
        private readonly GraphicsMenu _graphicsMenu;
        private readonly ISaveLoadService _saveLoadService;

        public PlayerPanelController(PanelSwitcher panelSwitcher, IGameFactory gameFactory,
            PlayerInputProvider playerInputProvider, GameplayMenu gameplayMenu, 
            LoadMenu loadMenu, SaveMenu saveMenu,
            SettingsMenu settingsMenu, GraphicsMenu graphicsMenu,
            ISaveLoadService saveLoadService,
            SoundMenu soundMenu, IEnemyRadiusChecker enemyRadiusChecker,
            StaticInventoryPanel staticInventoryPanel, SoundManger soundManger)
        {
            _panelSwitcher = panelSwitcher;
            _gameFactory = gameFactory;
            _playerInputProvider = playerInputProvider;
            _gameplayMenu = gameplayMenu;
            _loadMenu = loadMenu;
            _saveMenu = saveMenu;
            _settingsMenu = settingsMenu;
            _graphicsMenu = graphicsMenu;
            _saveLoadService = saveLoadService;
            _soundMenu = soundMenu;
            _enemyRadiusChecker = enemyRadiusChecker;
            _staticInventoryPanel = staticInventoryPanel;
            _soundManger = soundManger;
        }

        public void Initialize()
        {
            var savesList = FileManager.SavesList();
            var enumerable = savesList as string[] ?? savesList.ToArray();
            
            _saveMenu.Initialize(enumerable);
            _loadMenu.Initialize(enumerable);

            _gameFactory.UIObserver.GameplayMenu = _gameplayMenu;

            var settings = _saveLoadService.LoadSettings();
            
            _soundMenu.LoadAudioSettings(settings);
            _soundManger.SetMusicSound(settings.MusicVolume);
            _soundManger.SetEffectsSound(settings.EffectsVolume);

            _graphicsMenu.Init();
            _graphicsMenu.FindResolution(settings.ResolutionIndex);
            _graphicsMenu.SetFullscreen(Convert.ToBoolean(settings.IsFullScreen));
            _graphicsMenu.SetQualityLevelDropdown(settings.QualityLevel);
            
            _playerInputProvider.PlayerUIActions.Inventory.performed += EnableInventory;
            _playerInputProvider.PlayerUIActions.SkillBar.performed += EnableSkillBar;
            _playerInputProvider.PlayerUIActions.ESC.performed += TryToEnableMenu;

            _panelSwitcher.GetPanel<GameplayPanel>().NewSaveMenu.OnSaveClicked += OnSaveButtonClicked;
            _gameplayMenu.OnContinueButtonClicked += TryToEnableMenu;
            _saveMenu.OnSaveClicked += OnSaveButtonClicked;
            _soundMenu.OnAudioSettingsChanged += SaveAudioSettings;
            _graphicsMenu.OnGraphicsSettingsChanged += SaveGraphicsSettings;
        }

        public void Dispose()
        {
            _playerInputProvider.PlayerUIActions.Inventory.performed -= EnableInventory;
            _playerInputProvider.PlayerUIActions.SkillBar.performed -= EnableSkillBar;
            _playerInputProvider.PlayerUIActions.ESC.performed -= TryToEnableMenu;
            
            _gameplayMenu.OnContinueButtonClicked -= TryToEnableMenu;
            _saveMenu.OnSaveClicked -= OnSaveButtonClicked;
            _soundMenu.OnAudioSettingsChanged -= SaveAudioSettings;
            _graphicsMenu.OnGraphicsSettingsChanged -= SaveGraphicsSettings;
            _panelSwitcher.GetPanel<GameplayPanel>().NewSaveMenu.OnSaveClicked -= OnSaveButtonClicked;
        }

        private void SaveAudioSettings(float music, float effects)
        {
            _soundManger.SetMusicSound(music);
            _soundManger.SetEffectsSound(effects);
            
            _saveLoadService.SaveAudioSettings(music, effects);
        }

        private void SaveGraphicsSettings(int resolution, int isFullscreen, int graphics)
        {
            _saveLoadService.SaveGraphicsSettings(resolution, isFullscreen, graphics);
        }

        private void OnSaveButtonClicked(string saveFile)
        {
            if (_enemyRadiusChecker.IsEnemiesInRadius(
                    _gameFactory.PlayerObserver.PlayerInputProvider.transform.position, 10))
            {
                _staticInventoryPanel.WarningUI.Show("You can't save while enemies are nearby");
                return;
            }
            
            _saveLoadService.SaveProgress(saveFile);
            _staticInventoryPanel.WarningUI.Show("Game saved " + saveFile);

            _saveMenu.Clear();
            _loadMenu.Clear();
            
            var savesList = FileManager.SavesList();
            var enumerable = savesList as string[] ?? savesList.ToArray();
            
            _saveMenu.Initialize(enumerable);
            _loadMenu.Initialize(enumerable);
        }

        private void TryToEnableMenu(InputAction.CallbackContext obj)
        {
            TryToEnableMenu();
        }

        private void TryToEnableMenu()
        {
            var isHidden = _panelSwitcher.ChangeUIElement<GameplayPanel>(true);
            ShouldUseInput(isHidden);
        }

        private void EnableSkillBar(InputAction.CallbackContext obj)
        {
            var isHidden = _panelSwitcher.ChangeUIElement<SkillInventoryPanel>();
            ShouldUseInput(isHidden);
        }

        private void EnableInventory(InputAction.CallbackContext obj)
        {
            var isHidden = _panelSwitcher.ChangeUIElement<InventoryPanel>();
            ShouldUseInput(isHidden);
        }

        private void ShouldUseInput(bool isHidden)
        {
            if (isHidden)
            {
                _playerInputProvider.PlayerMainActions.Enable();
            }
            else
            {
                _playerInputProvider.PlayerMainActions.Disable();
            }
        }
    }
}
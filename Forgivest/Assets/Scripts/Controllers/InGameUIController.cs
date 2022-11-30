using System;
using System.Linq;
using GameCore.Factory;
using GameCore.SaveSystem.Reader;
using UI.Core;
using UI.Menu;
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

        public PlayerPanelController(PanelSwitcher panelSwitcher, IGameFactory gameFactory,
            PlayerInputProvider playerInputProvider, GameplayMenu gameplayMenu, 
            LoadMenu loadMenu, SaveMenu saveMenu,
            SettingsMenu settingsMenu)
        {
            _panelSwitcher = panelSwitcher;
            _gameFactory = gameFactory;
            _playerInputProvider = playerInputProvider;
            _gameplayMenu = gameplayMenu;
            _loadMenu = loadMenu;
            _saveMenu = saveMenu;
            _settingsMenu = settingsMenu;
        }

        public void Initialize()
        {
            _playerInputProvider.PlayerUIActions.Inventory.performed += EnableInventory;
            _playerInputProvider.PlayerUIActions.SkillBar.performed += EnableSkillBar;
            _playerInputProvider.PlayerUIActions.ESC.performed += TryToEnableMenu;

            var savesList = FileManager.SavesList();
            var enumerable = savesList as string[] ?? savesList.ToArray();
            
            _saveMenu.Initialize(enumerable);
            _loadMenu.Initialize(enumerable);

            _gameFactory.UIObserver.GameplayMenu = _gameplayMenu;
            
            _gameplayMenu.OnContinueButtonClicked += TryToEnableMenu;
        }

        public void Dispose()
        {
            _playerInputProvider.PlayerUIActions.Inventory.performed -= EnableInventory;
            _playerInputProvider.PlayerUIActions.SkillBar.performed -= EnableSkillBar;
            _playerInputProvider.PlayerUIActions.ESC.performed -= TryToEnableMenu;
            
            _gameplayMenu.OnContinueButtonClicked -= TryToEnableMenu;
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
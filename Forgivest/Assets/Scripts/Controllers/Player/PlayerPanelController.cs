using System;
using UI.Core;
using UI.Inventory.Core;
using UI.Skill;
using UnityEngine.InputSystem;
using Utilities;
using Zenject;

namespace Controllers.Player
{
    public class PlayerPanelController : IInitializable, IDisposable
    {
        private readonly PanelSwitcher _panelSwitcher;
        private readonly PlayerInputProvider _playerInputProvider;

        public PlayerPanelController(PanelSwitcher panelSwitcher, PlayerInputProvider playerInputProvider)
        {
            _panelSwitcher = panelSwitcher;
            _playerInputProvider = playerInputProvider;
        }

        public void Initialize()
        {
            _playerInputProvider.PlayerUIActions.Inventory.performed += EnableInventory;
            _playerInputProvider.PlayerUIActions.SkillBar.performed += EnableSkillBar;
        }

        public void Dispose()
        {
            _playerInputProvider.PlayerUIActions.Inventory.performed -= EnableInventory;
            _playerInputProvider.PlayerUIActions.SkillBar.performed -= EnableSkillBar;
        }

        private void EnableSkillBar(InputAction.CallbackContext obj)
        {
            var isHidden = _panelSwitcher.SwitchUIElement<SkillPanel>();
            ShouldUseInput(isHidden);
        }

        private void EnableInventory(InputAction.CallbackContext obj)
        {
            var isHidden = _panelSwitcher.SwitchUIElement<InventoryPanel>();
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
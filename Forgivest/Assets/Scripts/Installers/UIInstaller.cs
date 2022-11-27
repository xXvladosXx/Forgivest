using Controllers.Player;
using UI.Core;
using UI.Data;
using UI.HUD;
using UI.HUD.Stats;
using UI.Inventory.Core;
using UI.Menu;
using UI.Skill;
using UnityEngine;
using Zenject;
using InventoryPanel = UI.Inventory.Core.InventoryPanel;

namespace Installers
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private PanelSwitcher _panelSwitcher;
        [SerializeField] private InventoryPanel _inventoryPanel;
        [SerializeField] private SkillInventoryPanel skillInventoryPanel;
        [SerializeField] private StaticInventoryPanel staticInventoryPanel;

        [SerializeField] private HealthBarUI _healthBarUI;
        [SerializeField] private ManaBarUI _manaBarUI;
        [SerializeField] private ExperienceBarUI _experienceBarUI;
        
        [SerializeField] private GameplayMenu _gameplayMenu;
        [SerializeField] private LoadMenu _loadMenu;
        [SerializeField] private SaveMenu _saveMenu;
        [SerializeField] private SettingsMenu _settingsMenu;
        
        [SerializeField] private UIReusableData _uiReusableData;
        
        public override void InstallBindings()
        {
            Container.Bind<InventoryPanel>().FromInstance(_inventoryPanel).AsSingle();
            Container.Bind<HealthBarUI>().FromInstance(_healthBarUI).AsSingle();
            Container.Bind<ManaBarUI>().FromInstance(_manaBarUI).AsSingle();
            Container.Bind<SkillInventoryPanel>().FromInstance(skillInventoryPanel).AsSingle();
            Container.Bind<UIReusableData>().FromInstance(_uiReusableData).AsSingle();
            Container.Bind<StaticInventoryPanel>().FromInstance(staticInventoryPanel).AsSingle();
            Container.Bind<ExperienceBarUI>().FromInstance(_experienceBarUI).AsSingle();
            Container.Bind<PanelSwitcher>().FromInstance(_panelSwitcher).AsSingle();
            
            Container.Bind<GameplayMenu>().FromInstance(_gameplayMenu).AsSingle();
            Container.Bind<LoadMenu>().FromInstance(_loadMenu).AsSingle();
            Container.Bind<SaveMenu>().FromInstance(_saveMenu).AsSingle();
            Container.Bind<SettingsMenu>().FromInstance(_settingsMenu).AsSingle();
            
            Container.BindInterfacesAndSelfTo<PlayerPanelController>().AsSingle();
        }
    }
}
using UI.Data;
using UI.HUD;
using UI.HUD.Stats;
using UI.Inventory;
using UI.Inventory.Core;
using UI.Skill;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private InventoryPanel _inventoryPanel;
        [SerializeField] private SkillPanel _skillPanel;
        [SerializeField] private StaticPanel staticPanel;

        [SerializeField] private HealthBarUI _healthBarUI;
        [SerializeField] private ManaBarUI _manaBarUI;
        [SerializeField] private UIReusableData _uiReusableData;
        public override void InstallBindings()
        {
            Container.Bind<InventoryPanel>().FromInstance(_inventoryPanel).AsSingle();
            Container.Bind<HealthBarUI>().FromInstance(_healthBarUI).AsSingle();
            Container.Bind<ManaBarUI>().FromInstance(_manaBarUI).AsSingle();
            Container.Bind<SkillPanel>().FromInstance(_skillPanel).AsSingle();
            Container.Bind<UIReusableData>().FromInstance(_uiReusableData).AsSingle();
            Container.Bind<StaticPanel>().FromInstance(staticPanel).AsSingle();
        }
    }
}
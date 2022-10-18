using UI.Inventory;
using UI.Inventory.Core;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private InventoryPanel _inventoryPanel;
        public override void InstallBindings()
        {
            Container.Bind<InventoryPanel>().FromInstance(_inventoryPanel).AsSingle();
        }
    }
}
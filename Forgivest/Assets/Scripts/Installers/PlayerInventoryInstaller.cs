using Controllers;
using InventorySystem;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PlayerInventoryInstaller : MonoInstaller
    {
        [SerializeField] private Inventory _playerInventory;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InventoryController>().AsSingle();
            Container.Bind<Inventory>().FromInstance(_playerInventory).AsSingle();
        }
    }
}
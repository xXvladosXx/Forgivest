using Controllers;
using Controllers.Player;
using InventorySystem;
using InventorySystem.Interaction;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PlayerInventoryInstaller : MonoInstaller
    {
        [SerializeField] private ObjectPicker _playerInventory;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InventoryController>().AsSingle();
            Container.Bind<ObjectPicker>().FromInstance(_playerInventory).AsSingle();
        }
    }
}
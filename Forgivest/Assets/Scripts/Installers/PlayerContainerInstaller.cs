using AbilitySystem.AbilitySystem.Runtime.Abilities;
using Controllers;
using Controllers.Player;
using InventorySystem;
using InventorySystem.Interaction;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PlayerContainerInstaller : MonoInstaller
    {
        [SerializeField] private ObjectPicker _playerInventory;
        [SerializeField] private AbilityController _abilityController;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InventoryController>().AsSingle();
            Container.Bind<AbilityController>().FromInstance(_abilityController).AsSingle();
            Container.Bind<ObjectPicker>().FromInstance(_playerInventory).AsSingle();
        }
    }
}
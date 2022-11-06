using AbilitySystem.AbilitySystem.Runtime.Abilities;
using Controllers;
using Controllers.Player;
using InventorySystem;
using InventorySystem.Interaction;
using LevelSystem;
using Player;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PlayerContainerInstaller : MonoInstaller
    {
        [SerializeField] private ObjectPicker _playerInventory;
        [SerializeField] private AbilityHandler abilityHandler;
        [SerializeField] private LevelController _levelController;
        [SerializeField] private PlayerEntity _playerEntity;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InventoryController>().AsSingle();
            Container.BindInterfacesTo<PlayerAbilityController>().AsSingle();
            
            Container.Bind<AbilityHandler>().FromInstance(abilityHandler).AsSingle();
            Container.Bind<ObjectPicker>().FromInstance(_playerInventory).AsSingle();
            Container.Bind<LevelController>().FromInstance(_levelController).AsSingle();
            Container.Bind<PlayerEntity>().FromInstance(_playerEntity).AsSingle();
        }
    }
}
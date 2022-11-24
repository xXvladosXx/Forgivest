using AbilitySystem.AbilitySystem.Runtime.Abilities;
using AttackSystem.Core;
using Controllers;
using Controllers.Player;
using GameCore.StateMachine;
using InventorySystem;
using InventorySystem.Interaction;
using LevelSystem;
using LevelSystem.Scripts.Runtime;
using Logic;
using Player;
using StatSystem;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PlayerContainerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerEntity _playerEntity;
        [SerializeField] private Transform _startPosition;

        public override void InstallBindings()
        {
            var mainComponent = Container.InstantiatePrefabForComponent<PlayerEntity>(_playerEntity, _startPosition.position, Quaternion.identity, null);
            Container.BindInstance(mainComponent).AsSingle();

            Container.Bind<AbilityHandler>().FromInstance(mainComponent.AbilityHandler).AsSingle();
            Container.Bind<ObjectPicker>().FromInstance(mainComponent.ObjectPicker).AsSingle();
            Container.Bind<LevelController>().FromInstance(mainComponent.LevelController).AsSingle();
            Container.Bind<StatController>().FromInstance(mainComponent.StatController).AsSingle();

            Container.BindInterfacesAndSelfTo<InventoryController>().AsSingle();
            Container.BindInterfacesTo<PlayerAbilityController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerExperienceController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerHealthController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerManaController>().AsSingle();
        }
    }
}
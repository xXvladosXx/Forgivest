using GameCore;
using GameCore.AssetManagement;
using GameCore.Crutches;
using GameCore.Factory;
using GameCore.SaveSystem.Data;
using GameCore.SaveSystem.SaveLoad;
using GameCore.StateMachine;
using Logic;
using Player;
using SoundSystem;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameCoreInstaller : MonoInstaller
    {
        [SerializeField] private SoundManger _soundManger;
        
        public override void InstallBindings()
        {
            Container.Bind<GameBootstrapper>().FromNewComponentOnNewGameObject().AsSingle();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle().NonLazy();
            Container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.Bind<IPlayerObserver>().To<PlayerObserver>().AsSingle();
            
            Container.BindInstance(Container
                    .InstantiatePrefabForComponent<SoundManger>(_soundManger, Vector3.zero, Quaternion.identity, null))
                    .AsSingle();
            
            Container.Bind<IEnemyRadiusChecker>().To<EnemyRadiusChecker>().AsSingle();
        }
    }
}
using GameCore;
using GameCore.AssetManagement;
using GameCore.Data;
using GameCore.Data.SaveLoad;
using GameCore.Factory;
using GameCore.StateMachine;
using Logic;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameCoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameBootstrapper>().FromNewComponentOnNewGameObject().AsSingle();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle().NonLazy();
            Container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.Bind<IPlayerObserver>().To<PlayerObserver>().AsSingle();
        }
    }
}
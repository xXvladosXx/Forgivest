using Controllers.Player;
using StatSystem;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PlayerHealthInstaller : MonoInstaller
    {
        [SerializeField] private StatController _statController;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerHealthController>().AsSingle();
            Container.Bind<Health>().FromInstance(_statController.Health).AsSingle();
        }
    }
}
using Controllers.Player;
using StatSystem;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PlayerStatsInstaller : MonoInstaller
    {
        [SerializeField] private StatController _statController;
        public override void InstallBindings()
        {
            Container.Bind<StatController>().FromInstance(_statController).AsSingle();
            
            Container.BindInterfacesAndSelfTo<PlayerExperienceController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerHealthController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerManaController>().AsSingle();
        }
    }
}
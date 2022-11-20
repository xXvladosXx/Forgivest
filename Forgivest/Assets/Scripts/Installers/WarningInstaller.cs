using UI.Warning;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class WarningInstaller : MonoInstaller
    {
        [SerializeField] private ChooseWarning _chooseWarning;
        [SerializeField] private OneDecisionWarning _oneDecisionWarning;
        
        public override void InstallBindings()
        {
            Container.Bind<ChooseWarning>().FromInstance(_chooseWarning).AsSingle();
            Container.Bind<OneDecisionWarning>().FromInstance(_oneDecisionWarning).AsSingle();
        }
    }
}
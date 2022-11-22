using UI.Loading;
using UI.Utils;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class AdditionalDontDestroyOnLoadInstaller : MonoInstaller
    {
        [SerializeField] private GameObject[] _additionalDontDestroyOnLoadObjects;
        [SerializeField] private PersistentCanvas _persistentCanvas;

        public override void InstallBindings()
        {
            foreach (var additionalDontDestroyOnLoadObject in _additionalDontDestroyOnLoadObjects)
            {
                DontDestroyOnLoad(additionalDontDestroyOnLoadObject);
            }

            var canvas = Container.InstantiatePrefabForComponent<PersistentCanvas>(_persistentCanvas);
            Container.Bind<PersistentCanvas>().FromInstance(canvas).AsSingle();
        }
    }
}
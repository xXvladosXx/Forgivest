using UnityEngine;
using Zenject;

namespace Installers
{
    public class AdditionalDontDestroyOnLoadInstaller : MonoInstaller
    {
        [SerializeField] private GameObject[] _additionalDontDestroyOnLoadObjects;
        
        public override void InstallBindings()
        {
            foreach (var additionalDontDestroyOnLoadObject in _additionalDontDestroyOnLoadObjects)
            {
                DontDestroyOnLoad(additionalDontDestroyOnLoadObject);
            }
        }
    }
}
using System;
using UnityEngine;

namespace GameCore.SaveSystem.Scripts.Runtime
{
    [CreateAssetMenu(fileName = "LoadDataChannel", menuName = "SaveSystem/Channels/LoadDataChannel", order = 0)]
    public class LoadDataChannel : ScriptableObject
    {
        public event Action OnLoaded;

        public void Load()
        {
            OnLoaded?.Invoke();
        }
    }
}
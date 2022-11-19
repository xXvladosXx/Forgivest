using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace SaveSystem.Scripts.Runtime
{
    [CreateAssetMenu(fileName = "SaveDataChannel", menuName = "SaveSystem/Channels/SaveDataChannel", order = 0)]
    public class SaveDataChannel : ScriptableObject
    {
        private readonly List<SavableEntity> _savableEntities;
        public ReadOnlyCollection<SavableEntity> SavableEntities => _savableEntities.AsReadOnly();

        public event Action OnSaved;

        public void Add(SavableEntity savableEntity)
        {
            _savableEntities.Add(savableEntity);
        }

        public void Remove(SavableEntity savableEntity)
        {
            if (_savableEntities.Contains(savableEntity))
            {
                _savableEntities.Remove(savableEntity);
            }
        }
        
        public void Save()
        {
            OnSaved?.Invoke();
        }
    }
}
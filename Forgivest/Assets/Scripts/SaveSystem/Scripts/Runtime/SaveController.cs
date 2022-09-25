using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem.Scripts.Runtime
{
    public class SaveController : MonoBehaviour
    {
        [SerializeField] private SaveData _saveData;
        [SerializeField] private SaveDataChannel _saveDataChannel;
        [SerializeField] private LoadDataChannel _loadDataChannel;
        [SerializeField] private string _id;

        private void Reset()
        {
            _id = Guid.NewGuid().ToString();
        }

        private void OnEnable()
        {
            _loadDataChannel.OnLoaded += OnOnLoadedData;
            _saveDataChannel.OnSaved += OnOnSavedData;
        }

        private void OnDisable()
        {
            _loadDataChannel.OnLoaded -= OnOnLoadedData;
            _saveDataChannel.OnSaved -= OnOnSavedData;
        }

        private void OnOnSavedData()
        {
            var data = new Dictionary<string, object>();
            foreach (var savable in GetComponents<ISavable>())
            {
                data[savable.GetType().ToString()] = savable.Data;
            }
            _saveData.Save(_id, data);
        }

        private void OnOnLoadedData()
        {
            _saveData.Load(_id, out var data);
            var dictionary = data as Dictionary<string, object>;
            foreach (var savable in GetComponents<ISavable>())
            {
                savable.Load(dictionary[savable.GetType().ToString()]);
            }
        }
    }
}
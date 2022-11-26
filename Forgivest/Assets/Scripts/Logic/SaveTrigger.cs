using System;
using GameCore.Data.SaveLoad;
using UnityEngine;
using Zenject;

namespace Logic
{
    public class SaveTrigger : MonoBehaviour
    {
        private ISaveLoadService _saveLoadService;
        
        [Inject]
        public void Construct(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _saveLoadService.SaveProgress("NoGame");
                gameObject.SetActive(false);
            }
        }
    }
}
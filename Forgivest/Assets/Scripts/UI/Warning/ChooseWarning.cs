using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Warning
{
    public class ChooseWarning : BaseWarning
    {
        [SerializeField] private Button _yes;
        [SerializeField] private Button _no;

        public event Action OnAccepted;
        public event Action OnRejected;

        private void Awake()
        {
            Hide();
        }

        private void OnEnable()
        {
            _yes.onClick.AddListener(() =>
            {
                OnAccepted?.Invoke();
                Hide();
            });
            _no.onClick.AddListener(() =>
            {
                OnRejected?.Invoke();
                Hide();
            });
        }

        private void OnDisable()
        {
            _yes.onClick.RemoveAllListeners();
            _no.onClick.RemoveAllListeners();
        }

        
    }
}
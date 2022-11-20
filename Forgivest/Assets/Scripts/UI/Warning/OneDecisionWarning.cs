using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Warning
{
    public class OneDecisionWarning : BaseWarning
    {
        [SerializeField] private Button _ok;

        public event Action OnAccepted;

        private void Awake()
        {
            Hide();
        }

        private void OnEnable()
        {
            _ok.onClick.AddListener(() =>
            {
                OnAccepted?.Invoke();
                Hide();
            });
        }

        private void OnDisable()
        {
            _ok.onClick.RemoveAllListeners();
        }
    }
}
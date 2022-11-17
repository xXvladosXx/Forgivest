using System;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Loading
{
    public class LoadingScreen : UIElement
    {
        [field: SerializeField] public Image ProgressBarImage { get; private set; }
        [field: SerializeField] public GameObject LoadingBarObject { get; private set; }
        [field: SerializeField] public GameObject LoadingScreenObject { get; private set; }
        [field: SerializeField] public Button StartButton { get; private set; }

        public event Action OnStartGame;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            HideLoadingScreen();
        }

        private void OnEnable()
        {
            StartButton.onClick.AddListener(OnStartButtonClicked);
        }
        
        private void OnDisable()
        {
            StartButton.onClick.RemoveListener(OnStartButtonClicked);
        }

        public void LoadProgress(float progress)
        {
            ProgressBarImage.fillAmount = progress;

            if (ProgressBarImage.fillAmount == 1)
            {
                StartButton.gameObject.SetActive(true);
                HideLoadingBar();
            }
        }
        
        public void ShowLoadingScreen()
        {
            LoadingScreenObject.SetActive(true);
        }
        
        public void HideLoadingScreen()
        {
            LoadingScreenObject.SetActive(false);
        }
        
        public void ShowLoadingBar()
        {
            LoadingBarObject.SetActive(true);
        }
        
        public void HideLoadingBar()
        {
            LoadingBarObject.SetActive(false);
        }
        
        public void OnStartButtonClicked()
        {
            OnStartGame?.Invoke();
            StartButton.gameObject.SetActive(false);
        }
    }
}
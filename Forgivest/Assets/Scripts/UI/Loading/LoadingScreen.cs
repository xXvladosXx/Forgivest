﻿using System;
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
        [field: SerializeField] public InputChecker.InputChecker InputChecker { get; private set; }

        public event Action OnStartGame;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            HideLoadingScreen();
        }

        private void OnEnable()
        {
            InputChecker.OnHidden += OnInteracted;
        }
        
        private void OnDisable()
        {
            InputChecker.OnHidden -= OnInteracted;
        }

        public void LoadProgress(float progress)
        {
            ProgressBarImage.fillAmount = progress;

            if (ProgressBarImage.fillAmount == 1)
            {
                InputChecker.gameObject.SetActive(true);
                HideLoadingBar();
            }
        }
        
        public void ShowLoadingScreen()
        {
            LoadingScreenObject.SetActive(true);
            InputChecker.gameObject.SetActive(false);
        }
        
        public void HideLoadingScreen()
        {
            LoadingScreenObject.SetActive(false);
            InputChecker.gameObject.SetActive(false);
        }
        
        public void ShowLoadingBar()
        {
            LoadingBarObject.SetActive(true);
        }
        
        public void HideLoadingBar()
        {
            LoadingBarObject.SetActive(false);
        }
        
        public void OnInteracted()
        {
            OnStartGame?.Invoke();
            InputChecker.gameObject.SetActive(false);
        }
    }
}
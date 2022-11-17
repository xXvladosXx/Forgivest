using DG.Tweening;
using UI.Core;
using UnityEngine;

namespace UI.Utils
{
    public class Fader : UIElement
    {
        [SerializeField] private CanvasGroup _fader;
        [SerializeField] protected float FadeTime = 1f;
        
        protected void StartFading(float to)
        {
            if (to == 0)
            {
                _fader.DOFade(to, FadeTime).OnComplete(OnFadeCompleted);
            }
            else
            {
                _fader.DOFade(to, FadeTime).OnComplete(OnFadeStarted);
            }
        }

        public virtual void OnFadeCompleted() {  }
        public virtual void OnFadeStarted() {  }
    }
}
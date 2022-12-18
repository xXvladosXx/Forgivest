using DG.Tweening;
using TMPro;
using UI.Core;
using UnityEngine;

namespace UI.HUD
{
    public class WarningUI : UIElement
    {
        [SerializeField] private float _timeToHide = 2f;
        [SerializeField] private float _timeToShow = .5f;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        private Tweener _tweener;
        public void Show(string text)
        {
            _text.text = text;
            
            if(_tweener != null) return;
            
            _tweener = _canvasGroup.DOFade(1, _timeToShow).OnComplete(OnFadeCompleted);
        }

        private void OnFadeCompleted()
        {
            _canvasGroup.DOFade(0, _timeToHide).OnComplete(() =>
            {
                _tweener.Kill();
                _tweener = null;
            });
        }
    }
}
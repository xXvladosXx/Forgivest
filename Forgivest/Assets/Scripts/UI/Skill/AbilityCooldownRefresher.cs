using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Skill
{
    public class AbilityCooldownRefresher : UIElement
    {
        [SerializeField] private Image _refreshIcon;
        
        public void SetFillAmountToZero()
        {
            _refreshIcon.fillAmount = 0;
        }
        
        public void RefreshImage(float timeLeft, float maxTime)
        {
            _refreshIcon.fillAmount = timeLeft / maxTime;
        }
    }
}
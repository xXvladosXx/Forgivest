using UnityEngine;

namespace UI.Utils
{
    public class ToFade : Fader
    {
        [SerializeField] private float _to;
        
        public void TriggeredDeath()
        {
            StartFading(_to);
        }

        public void Reset()
        {
            StartFading(0);
        }
    }
}
using System;
using System.Collections;
using UnityEngine;

namespace UI.Utils
{
    public class LoopingFader : Fader
    {
        private void OnEnable()
        {
            OnFadeStarted();
        }
        
        public override void OnFadeCompleted()
        {
            StartFading(1);
        }

        public override void OnFadeStarted()
        {
            StartFading(0);
        }
    }
}
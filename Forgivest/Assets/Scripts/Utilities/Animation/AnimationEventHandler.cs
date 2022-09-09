using UnityEngine;

namespace Utilities.Animation
{
    public class AnimationEventHandler : MonoBehaviour
    {
        [field: SerializeField] public IAnimationEventUser AnimationEventUser { get; private set; }
        
        public void OnAnimationStart()
        {
            AnimationEventUser.OnAnimationStarted();
        }

        public void OnAnimationTransition()
        {
            AnimationEventUser.OnAnimationTransitioned();
        }

        public void OnAnimationEnd()
        {
            AnimationEventUser.OnAnimationEnded();
        }
    }
}
using UnityEngine;

namespace AnimationSystem
{
    public class AnimationChanger
    {
        private readonly Animator _animator;

        public AnimationChanger(Animator animator)
        {
            _animator = animator;
        }
        
        public void StartAnimation(int animationHash)
        {
            _animator.SetBool(animationHash, true);
        }

        public void UpdateBlendAnimation(int animationHash, float speed, float dampTime)
        {
            _animator.SetFloat(animationHash, speed, dampTime, Time.deltaTime);
        }
        
        public void StopAnimation(int animationHash)
        {
            _animator.SetBool(animationHash, false);
        }

        public void ChangeRuntimeAnimatorController(RuntimeAnimatorController weaponAnimatorController)
        {
            _animator.runtimeAnimatorController = weaponAnimatorController;
        }
    }
}
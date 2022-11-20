using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnimationSystem
{
    public class AnimationEventHandler : SerializedMonoBehaviour
    {
        [field: SerializeField] public IAnimationEventUser AnimationEventUser { get; private set; }

        private void Awake()
        {
            AnimationEventUser ??= GetComponent<IAnimationEventUser>();
        }

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

        public void ApplyDamage(float colliderActivationTime)
        {
            AnimationEventUser.ApplyAttack(colliderActivationTime);
        }

        public void Cast()
        {
            AnimationEventUser.CastedSkill();
        }
        
        public void Shoot()
        {
            AnimationEventUser.CastedProjectile();
        }

        public void Spawn()
        {
            AnimationEventUser.CastedSpawn();
        }
    }
}
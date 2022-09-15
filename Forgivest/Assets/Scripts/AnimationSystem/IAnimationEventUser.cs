using Data.Player;

namespace AnimationSystem
{
    public interface IAnimationEventUser
    {
        public AliveEntityAnimationData AliveEntityAnimationData { get; }
        void OnAnimationStarted();
        void OnAnimationTransitioned();
        void OnAnimationEnded();
        void ApplyAttack(float timeOfActiveCollider);
    }
}
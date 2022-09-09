namespace Utilities.Animation
{
    public interface IAnimationEventUser
    {
        public void OnAnimationStarted();
        public void OnAnimationTransitioned();
        public void OnAnimationEnded();
    }
}
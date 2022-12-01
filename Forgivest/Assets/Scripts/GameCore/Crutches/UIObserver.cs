using UI.Menu;

namespace GameCore.Crutches
{
    public class UIObserver : IUIObserver
    {
        public GameplayMenu GameplayMenu { get; set; }
        public SaveMenu SaveMenu { get; set; }
    }
}
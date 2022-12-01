using UI.Menu;

namespace GameCore.Crutches
{
    public interface IUIObserver
    {
        GameplayMenu GameplayMenu { get; set; }
        SaveMenu SaveMenu { get; set; }
    }
}
namespace Utilities.UI.Dragging
{
    public interface IDragSource<out T> where T : class
    {
        T GetItem();
        int GetNumber();
        void RemoveItems(int number);
    }
}
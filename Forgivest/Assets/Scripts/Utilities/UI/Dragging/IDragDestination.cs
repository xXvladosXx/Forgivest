namespace Utilities.UI.Dragging
{
    public interface IDragDestination<in T> where T : class
    {
        int MaxAcceptable(T item);
        void AddItems(T item, int number);
    }
}
using UnityEngine;

namespace Utilities.UI.Dragging
{
    public interface IDragSource 
    {
        int SourceIndex { get; }
        Sprite GetItem();
        int GetNumber();
        void RemoveItems(int number);
    }
}
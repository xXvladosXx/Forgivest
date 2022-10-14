using UnityEngine;

namespace UI.Inventory.Core
{
    public interface IDragSource 
    {
        int SourceIndex { get; }
        Sprite GetIcon();
        int GetNumber();
        void RemoveItems(int number);
    }
}
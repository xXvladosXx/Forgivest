using UnityEngine;

namespace Utilities.UI.Dragging
{
    public interface IDragDestination 
    {
        int Index { get; }
        int MaxAcceptable(Sprite item);
        public void AddItems(Sprite itemSprite, int number, int index);
    }
}
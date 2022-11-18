using UnityEngine;

namespace RaycastSystem.Core
{
    public interface IRaycastUser
    {
        public RaycastHit? RaycastHit { get; } 
        public RaycastHit? Raycast(Vector2 pointFrom);
        public RaycastHit? RaycastExcept(Vector2 pointFrom, LayerMask layerMask);
        PlayerRaycastSettings.CursorMapping GetCursorMapping(CursorType type);
        public void SetCursor(CursorType cursorType)
        {
            var mapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(mapping.Texture, mapping.Hotspot, CursorMode.Auto);
        }
    }
}
using UnityEngine;

namespace RaycastSystem.Core
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        bool HandleRaycast(IRaycastUser raycastUser);
        GameObject GameObject { get; }
    }
}
using UnityEngine;

namespace RaycastSystem.Core
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        bool HandleRaycast(RaycastUser raycastUser);
        GameObject GameObject { get; }
    }
}
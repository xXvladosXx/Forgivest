using UnityEngine;

namespace RaycastSystem.Core
{
    public abstract class RaycastUser
    {
        public RaycastHit? RaycastHit { get; } = null;
        public abstract RaycastHit? Raycast(Vector2 pointFrom);
        public abstract RaycastHit? RaycastExcept(Vector2 pointFrom, LayerMask layerMask);
    }
}
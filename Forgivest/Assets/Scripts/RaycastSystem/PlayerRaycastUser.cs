using RaycastSystem.Core;
using StateMachine.Player;
using UnityEngine;
using Utilities;

namespace RaycastSystem
{
    public class PlayerRaycastUser : RaycastUser
    {
        private readonly Camera _camera;

        public PlayerRaycastUser(Camera camera)
        {
            _camera = camera;
        }
        
        public override RaycastHit? Raycast(Vector2 pointFrom)
        {
            var ray = _camera.ScreenPointToRay(pointFrom);
            bool hasHit = Physics.Raycast(ray, out var raycastHit, Mathf.Infinity);

            if (hasHit)
                return raycastHit;

            return null;
        }

        public override RaycastHit? RaycastExcept(Vector2 pointFrom, LayerMask layerMask)
        {
            var ray = _camera.ScreenPointToRay(pointFrom);
            bool hasHit = Physics.Raycast(ray, out var raycastHit, Mathf.Infinity, layerMask);
            
            if (hasHit)
                return raycastHit;

            return null;
        }
    }
}
using System;
using MovementSystem;
using RaycastSystem.Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Utilities;

namespace RaycastSystem
{
    public class PlayerRaycastUser : RaycastUser
    {
        private readonly PlayerInputProvider _playerInputProvider;
        private readonly PlayerRaycastSettings _playerRaycastSettings;
        private readonly Movement _movement;
        private readonly Camera _camera;

        private RaycastHit[] _possibleRaycasts;
        
        private const float RAYCAST_RADIUS = 1f;
        private const int RAYCAST_ARRAY_SIZE = 100;

        public RaycastHit? RaycastHit { get; private set; }

        public PlayerRaycastUser(Camera camera,
            PlayerInputProvider playerInputProvider,
            PlayerRaycastSettings playerRaycastSettings,
            Movement movement)
        {
            _camera = camera;
            _playerInputProvider = playerInputProvider;
            _playerRaycastSettings = playerRaycastSettings;
            _movement = movement;
        }

        public void Tick()
        {
            RaycastHit = RaycastExcept(_playerInputProvider.ReadMousePosition(), LayerUtils.Player);
            
            if (InteractWithUI()) return;
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            
            SetCursor(CursorType.UI);
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                if (!_movement.CanMoveTo(target)) return false;

                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }
        
        RaycastHit[] RaycastAllSorted()
        {
            var ray = _camera.ScreenPointToRay(_playerInputProvider.ReadMousePosition());
            RaycastHit[] hits = Physics.SphereCastAll(ray, RAYCAST_RADIUS);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }
        
        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private void SetCursor(CursorType cursorType)
        {
            var mapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(mapping.Texture, mapping.Hotspot, CursorMode.Auto);
        }
        
        private PlayerRaycastSettings.CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (PlayerRaycastSettings.CursorMapping mapping in _playerRaycastSettings.CursorMappings)
            {
                if (mapping.Type == type)
                {
                    return mapping;
                }
            }
            return _playerRaycastSettings.CursorMappings[0];
        }
        
        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
             
            var hasHit = RaycastHit.HasValue;
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            var hasCastToNavMesh = NavMesh.SamplePosition(
                RaycastHit.Value.point, out navMeshHit,
                _playerRaycastSettings.MaxNavMeshProjectionDistance,
                NavMesh.AllAreas);
            
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            return true;
        }

        public override RaycastHit? Raycast(Vector2 pointFrom)
        {
            var ray = _camera.ScreenPointToRay(pointFrom);
            var hasHit = Physics.Raycast(ray, out var raycastHit, Mathf.Infinity);

            if (hasHit)
                return raycastHit;

            return null;
        }

        public override RaycastHit? RaycastExcept(Vector2 pointFrom, LayerMask layerMask)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return null;
            }
            
            var ray = _camera.ScreenPointToRay(pointFrom);
            var hasHit = Physics.Raycast(ray, out var raycastHit, Mathf.Infinity, layerMask);
            
            if (hasHit)
                return raycastHit;

            return null;
        }
    }
}
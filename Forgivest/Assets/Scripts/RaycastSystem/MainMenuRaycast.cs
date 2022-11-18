using RaycastSystem.Core;
using UnityEngine;
using Utilities;

namespace RaycastSystem
{
    public class MainMenuRaycast : MonoBehaviour, IRaycastUser
    {
        [SerializeField] private PlayerInputProvider _playerInputProvider;
        [SerializeField] private PlayerRaycastSettings _playerRaycastSettings;
        [SerializeField] private Camera _camera;

        public RaycastHit? RaycastHit { get; }

        private void Update()
        {
            (this as IRaycastUser).SetCursor(CursorType.UI);
        }

        public RaycastHit? Raycast(Vector2 pointFrom)
        {
            return null;
        }

        public RaycastHit? RaycastExcept(Vector2 pointFrom, LayerMask layerMask)
        {
            return null;
        }

        public PlayerRaycastSettings.CursorMapping GetCursorMapping(CursorType type)
        {
            return _playerRaycastSettings.CursorMappings[0];
        }
    }
}
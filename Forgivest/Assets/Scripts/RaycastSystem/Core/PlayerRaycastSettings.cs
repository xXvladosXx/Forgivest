using UnityEngine;

namespace RaycastSystem.Core
{
    [CreateAssetMenu(fileName = "PlayerRaycast", menuName = "Raycast/PlayerRaycast", order = 0)]
    public class PlayerRaycastSettings : ScriptableObject
    {
        [field: SerializeField] public CursorMapping[] CursorMappings { get; private set; }
        [field: SerializeField] public float MaxNavMeshProjectionDistance { get; private set; } = 1f;

        [System.Serializable]
        public struct CursorMapping
        {
            [field: SerializeField] public CursorType Type { get; set; }
            [field: SerializeField] public Texture2D Texture { get; set; }
            [field: SerializeField] public Vector2 Hotspot { get; set; }
        }
    }
}
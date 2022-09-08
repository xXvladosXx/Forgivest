using System;
using UnityEngine;

namespace Data.Player
{
    [Serializable]
    public class PlayerLayerData
    {
        [field: SerializeField] public LayerMask GroundLayer { get; private set; }
        [field: SerializeField] public LayerMask MouseColliderLayerMask { get; private set; }
        [field: SerializeField] public LayerMask EnemyLayer { get; private set; }
        [field: SerializeField] public LayerMask UninteractableLayer { get; private set; }

        public bool ContainsLayer(LayerMask layerMask, int layer)
        {
            return (1 << layer & layerMask) != 0;
        }

        public bool IsGroundLayer(int layer)
        {
            return ContainsLayer(GroundLayer, layer);
        }
    }
}
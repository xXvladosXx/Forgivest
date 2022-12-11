using System;
using UnityEngine;

namespace InventorySystem.Items
{
    [Serializable]
    public class PointsRequirement : IRequirement
    {
        [field: SerializeField] public int NecessaryPoints { get; private set; }
        public string Description => $"Requires points: {NecessaryPoints}";
    }
}
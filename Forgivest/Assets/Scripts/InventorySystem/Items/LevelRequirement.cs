﻿using UnityEngine;

namespace InventorySystem.Items
{
    public class LevelRequirement : IRequirement
    {
        [field: SerializeField] public int NecessaryLevel { get; private set; }
    }
}
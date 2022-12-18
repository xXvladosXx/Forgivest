using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core;
using UnityEngine;

namespace InventorySystem.Items.Weapon
{
    public abstract class Weapon : StatsableItem, ITaggable
    {
        [field: SerializeField] public bool RightHanded { get; private set; }
        [field: SerializeField] public float AttackRate { get; private set; }
        [field: SerializeField] public RuntimeAnimatorController AnimatorController { get; private set; }
        [field: SerializeField] public List<string> PossibleTags { get; private set; }
        public ReadOnlyCollection<string> Tags => PossibleTags.AsReadOnly();
    }
}
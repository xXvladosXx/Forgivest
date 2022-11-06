using Core.Nodes;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Core
{
    public class AbilityLevelNode : CodeFunctionNode
    {
        [SerializeField] private string _abilityName;
        public string AbilityName => _abilityName;
        public Ability Ability;
        public override float Value => Ability.Level;
        public override float CalculateValue(GameObject source)
        {
            var abilityController = source.GetComponent<AbilityHandler>();
            return abilityController.Abilities[_abilityName].Level;
        }
    }
}
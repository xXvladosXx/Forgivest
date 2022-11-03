using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Core;
using AttackSystem;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Active
{
    [AbilityType(typeof(RadiusDamageAbility))]
    [CreateAssetMenu (menuName = "AbilitySystem/Ability/RadiusDamageAbility")]
    public class RadiusDamageAbilityDefinition : ActiveAbilityDefinition
    {
        [SerializeField] private Vector3 _offset;
        public Vector3 Offset => _offset;
        
        [SerializeField] private bool _facingPointCast;
        public bool FacingPointCast => _facingPointCast;
        
        [SerializeField] private RadiusDamager _radiusDamager;
        public RadiusDamager RadiusDamager => _radiusDamager;

        [SerializeField] private Vector3 _playerPosition;
        public Vector3 PlayerPosition => _playerPosition;
        [SerializeField] private bool _spawnOnPlayerPosition;
        public bool SpawnOnPlayerPosition => _spawnOnPlayerPosition;
    }
}
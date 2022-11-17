using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Core;
using AttackSystem;
using AttackSystem.Core;
using CombatSystem.Scripts.Runtime;
using StatSystem.Scripts.Runtime;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Active
{
    [AbilityType(typeof(ProjectileAbility))]
    [CreateAssetMenu (menuName = "AbilitySystem/Ability/ProjectileAbility")]
    public class ProjectileAbilityDefinition : ActiveAbilityDefinition
    {
        [SerializeField] private float _speed = 10f;
        public float Speed => _speed;
        
        [SerializeField] private ShotType _shotType = ShotType.MOST_DIRECT;
        public ShotType ShotType => _shotType;
        
        [SerializeField] private bool _isSpin = false;
        public bool IsSpin => _isSpin;

        [SerializeField] private Projectile _projectile;
        public Projectile Projectile => _projectile;
        
        [SerializeField] private string _weaponID;
        public string WeaponID => _weaponID;
    }
}
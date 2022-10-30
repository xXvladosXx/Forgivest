using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using AttackSystem;
using AttackSystem.Core;
using CombatSystem.Scripts.Runtime;
using CombatSystem.Scripts.Runtime.Core;
using StatSystem.Scripts.Runtime;
using UnityEngine;
using UnityEngine.Pool;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Active
{
    public class ProjectileAbility : ActiveAbility
    {
        public new ProjectileAbilityDefinition Definition => AbilityDefinition as ProjectileAbilityDefinition;
        private ObjectPool<Projectile> _objectPool;
        private AttackData _attackData;

        public ProjectileAbility(ProjectileAbilityDefinition definition, AbilityController abilityController, AttackData attackData) : base(
            definition, abilityController, attackData)
        {
            _objectPool = new ObjectPool<Projectile>(OnCreate, OnGet, OnRelease);
        }

        private Projectile OnCreate()
        {
            var projectile = Object.Instantiate(Definition.Projectile);
            projectile.OnHit += OnHit;
            return projectile;
        }

        private void OnHit(CollisionData collisionData)
        {
            _attackData.DamageReceiver = collisionData
                .Target
                .TryGetComponent(out IDamageReceiver damageReceiver) ? damageReceiver : null;

            OnRelease(collisionData.Source as Projectile);
            ApplyEffects(collisionData.Target);
            
            _attackData.DamageReceiver?.ReceiveDamage(_attackData);
        }

        private void OnGet(Projectile obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void OnRelease(Projectile obj)
        {
            obj.Rigidbody.velocity = Vector3.zero;
            obj.gameObject.SetActive(false);
        }

        public void Shoot(GameObject target, IDamageApplier damageApplier)
        {
            _attackData = new AttackData
            {
                DamageApplier = damageApplier,
            };

            var projectile = _objectPool.Get();
            damageApplier.ApplyShoot(
                projectile,
                target.transform,
                Definition.Speed,
                Definition.ShotType,
                Definition.IsSpin);
        }
    }
}
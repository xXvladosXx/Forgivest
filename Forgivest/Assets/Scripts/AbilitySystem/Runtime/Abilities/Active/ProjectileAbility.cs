using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using AttackSystem.Core;
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

        public ProjectileAbility(ProjectileAbilityDefinition definition, AbilityController abilityController) : base(
            definition, abilityController)
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
            OnRelease(collisionData.Source as Projectile);
            ApplyEffects(collisionData.Target, _attackData);
            
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

        public void Shoot(AttackData attackData)
        {
            var projectile = _objectPool.Get();

            _attackData = attackData;
            _attackData.DamageApplier.ApplyShoot(
                projectile,
                _attackData.DamageReceiver.GameObject.transform,
                Definition.Speed,
                Definition.ShotType,
                Definition.IsSpin);
        }
    }
}
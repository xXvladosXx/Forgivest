using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
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
        protected CombatController CombatController;

        public ProjectileAbility(ProjectileAbilityDefinition definition, AbilityController abilityController) : base(definition, abilityController)
        {
            _objectPool = new ObjectPool<Projectile>(OnCreate, OnGet, OnRelease);
            CombatController = abilityController.GetComponent<CombatController>();
        }

        private Projectile OnCreate()
        {
            var projectile = Object.Instantiate(Definition.Projectile);
            projectile.OnHit += OnHit;
            return projectile;
        }

        private void OnHit(CollisionData data)
        {
            OnRelease(data.Source as Projectile);
            ApplyEffects(data.Target);
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

        public void Shoot(GameObject target)
        {
            if (CombatController.RangedWeapons.TryGetValue(Definition.WeaponID, out var weapon))
            {
                var projectile = _objectPool.Get();
                weapon.Shoot(
                    projectile,
                    target.transform,
                    Definition.Speed,
                    Definition.ShotType,
                    Definition.IsSpin);
            }
        }
    }
}
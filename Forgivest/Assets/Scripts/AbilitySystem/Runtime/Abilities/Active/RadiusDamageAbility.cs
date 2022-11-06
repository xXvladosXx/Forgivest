using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using AttackSystem;
using AttackSystem.Core;
using UnityEngine;
using UnityEngine.Pool;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Active
{
    public class RadiusDamageAbility : ActiveAbility
    {
        public new RadiusDamageAbilityDefinition Definition => AbilityDefinition as RadiusDamageAbilityDefinition;
        private ObjectPool<RadiusDamager> _objectPool;
        private AttackData _attackData;

        public RadiusDamageAbility(ActiveAbilityDefinition definition, AbilityHandler abilityHandler) : base(definition, abilityHandler)
        {
            _objectPool = new ObjectPool<RadiusDamager>(OnCreate, OnGet, OnRelease);
        }

        private RadiusDamager OnCreate()
        {
            var radiusDamager = Object.Instantiate(Definition.RadiusDamager);
            radiusDamager.OnHit += OnHit;
            
            return radiusDamager;
        }
      
        private void OnHit(CollisionData collisionData)
        {
            if(collisionData.Target.layer == _attackData.DamageApplierLayerMask)
                return;
            
            _attackData.DamageReceiver = collisionData
                .Target
                .TryGetComponent(out IDamageReceiver damageReceiver) ? damageReceiver : null;

            ApplyEffects(collisionData.Target, _attackData);
            _attackData.DamageReceiver?.ReceiveDamage(_attackData);
        }

        private void OnGet(RadiusDamager obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void OnRelease(RadiusDamager obj)
        {
            obj.gameObject.SetActive(false);
        }

        public void Spawn(Vector3 clickedPoint, AttackData attackData)
        {
            _attackData = attackData;
            
            var radiusDamager = _objectPool.Get();
            radiusDamager.VisualEffect.Play();
            var pointToSpawn = clickedPoint + Definition.Offset;

            if (Definition.SpawnOnPlayerPosition)
            {
                pointToSpawn = ((GameObject)attackData.Source).transform.position;
            }

            radiusDamager.transform.rotation = ((GameObject)attackData.Source).transform.rotation;
            pointToSpawn.y = 0;
            radiusDamager.transform.position = pointToSpawn;
            
            radiusDamager.VisualEffect.OnFinished += e =>
            {
                ShouldRelease(radiusDamager);
            };
        }

        private void ShouldRelease(RadiusDamager obj)
        {
            OnRelease(obj);
        }
    }
}
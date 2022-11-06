using System.Collections;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using AttackSystem.Core;
using MovementSystem;
using UnityEngine;
using Utilities;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Active
{
    public class AttackAbility : ActiveAbility
    {
        public new AttackAbilityDefinition Definition => AbilityDefinition as AttackAbilityDefinition;

        private Coroutine _coroutine;
        private Movement _movement;
        
        public AttackAbility(ActiveAbilityDefinition definition, AbilityHandler abilityHandler) : base(definition, abilityHandler)
        {
        }

        public void OnEnter(Movement movement, Vector3 target)
        {
            _movement = movement;
            
            ApplyEffects(movement.Transform.gameObject, null);

            if (_coroutine != null)
            {
                Coroutines.StopRoutine(_coroutine);
            }
            
            _coroutine = Coroutines.StartRoutine(StopDelay());

            movement.MoveTo(target + movement.Transform.forward * Definition.DistanceModifier, Definition.Speed);
        }

        public void OnUpdate()
        {
           
        }
        
        private IEnumerator StopDelay()
        {
            yield return new WaitForSeconds(Definition.TimeToStop);
            _movement.Stop();
        }

        public void OnExit()
        {
            Coroutines.StopRoutine(_coroutine);
            _movement.Stop();
        }
    }
}
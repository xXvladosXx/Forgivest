using System;
using System.Collections.Generic;
using AbilitySystem.AbilitySystem.Runtime;
using AbilitySystem.AbilitySystem.Runtime.VisualEffects;
using Core;
using UnityEngine;

namespace AbilitySystem
{
    public partial class GameplayEffectController
    {
        private Dictionary<SpecialEffectDefinition, int> _specialEffectCount =
            new Dictionary<SpecialEffectDefinition, int>();
        
        private Dictionary<SpecialEffectDefinition, VisualEffect> _specialEffectDictionary =
            new Dictionary<SpecialEffectDefinition, VisualEffect>();

        private void PlaySpecialEffect(GameplayPersistentEffect effect)
        {
            var visualEffect =
                Instantiate(effect.Definition.SpecialPersistentEffectDefinition.Prefab, transform);

            visualEffect.OnFinished += e => Destroy(e.gameObject);

            visualEffect.transform.localPosition =
                effect.Definition.SpecialPersistentEffectDefinition.Location switch
                {
                    PlayLocation.Center => ComponentHeight.GetCenterOfCollider(transform),
                    PlayLocation.Above => ComponentHeight.GetComponentHeight(gameObject) * Vector3.up,
                    
                    _ => visualEffect.transform.localPosition
                };

            if (visualEffect.IsLooping)
            {
                if (_specialEffectDictionary.ContainsKey(effect.Definition.SpecialPersistentEffectDefinition))
                {
                    _specialEffectCount[effect.Definition.SpecialPersistentEffectDefinition]++;
                }
                else
                {
                    _specialEffectCount.Add(effect.Definition.SpecialPersistentEffectDefinition, 1);
                    _specialEffectDictionary.Add(effect.Definition.SpecialPersistentEffectDefinition, visualEffect);
                }
            }
            
            visualEffect.Play();
        }

        private void PlaySpecialEffect(GameplayEffect effect)
        {
            var visualEffect = Instantiate(effect.Definition.SpecialEffectDefinition.Prefab,
                transform.position, transform.rotation);

            visualEffect.OnFinished += e => Destroy(e.gameObject);

            switch (effect.Definition.SpecialEffectDefinition.Location)
            {
                case PlayLocation.Center:
                    visualEffect.transform.position = ComponentHeight.GetComponentHeight(gameObject) * Vector3.up;
                    break;
                case PlayLocation.Above:
                    visualEffect.transform.position = ComponentHeight.GetCenterOfCollider(transform);
                    break;
            }
            
            visualEffect.Play();
        }

        private void StopSpecialEffect(GameplayPersistentEffect effect)
        {
            if (_specialEffectCount.ContainsKey(effect.Definition.SpecialPersistentEffectDefinition))
            {
                _specialEffectCount[effect.Definition.SpecialPersistentEffectDefinition]--;
                if (_specialEffectCount[effect.Definition.SpecialPersistentEffectDefinition] == 0)
                {
                    _specialEffectCount.Remove(effect.Definition.SpecialPersistentEffectDefinition);
                    var visualEffect = _specialEffectDictionary[effect.Definition.SpecialPersistentEffectDefinition];
                    visualEffect.Stop();
                    _specialEffectDictionary.Remove(effect.Definition.SpecialPersistentEffectDefinition);
                }
            }
            else
            {
                Debug.Log("No effect to remove");
            }
        }
    }
}
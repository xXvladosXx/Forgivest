using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.VisualEffects
{
    public enum PlayLocation
    {
        Above,
        Center,
        Below
    }
    
    [CreateAssetMenu (menuName = "Core/SpecialEffect")]
    public class SpecialEffectDefinition : ScriptableObject
    {
        [field: SerializeField] public PlayLocation Location { get; private set; }
        [field: SerializeField] public VisualEffect Prefab { get; private set; }
        
    }
}
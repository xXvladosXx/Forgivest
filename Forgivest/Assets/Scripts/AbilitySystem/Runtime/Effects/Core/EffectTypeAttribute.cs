
using System;

namespace AbilitySystem.AbilitySystem.Runtime.Effects.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class EffectTypeAttribute : Attribute
    {
        public readonly Type Type;

        public EffectTypeAttribute(Type type)
        {
            Type = type;
        }
    }
}
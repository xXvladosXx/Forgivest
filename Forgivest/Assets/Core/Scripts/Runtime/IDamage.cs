using UnityEngine;

namespace Core
{
    public interface IDamage
    {
        bool IsCriticalHit { get; }
        float Magnitude { get; }
        GameObject Instigator { get; }
        object Source { get; }
    }
}
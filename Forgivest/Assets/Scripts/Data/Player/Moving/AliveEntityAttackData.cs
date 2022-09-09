using System;
using UnityEngine;

namespace Data.Player.Moving
{
    [Serializable]
    public class AliveEntityAttackData
    {
        [field: SerializeField] public float StoppingDistance { get; private set; } = 0;
    }
}
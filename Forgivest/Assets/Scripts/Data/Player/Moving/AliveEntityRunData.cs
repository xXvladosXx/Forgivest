using System;
using UnityEngine;

namespace Data.Player.Moving
{
    [Serializable]
    public class AliveEntityRunData
    {
        [field: SerializeField] [field: Range(1f, 2f)] public float SpeedModifier { get; private set; } = 1f;
        [field: SerializeField] public float SmoothInputSpeed { get; private set; } = 1f;
        [field: SerializeField] public float StoppingDistance { get; private set; } = 0;
    }
}
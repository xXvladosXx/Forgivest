using System;
using Characters.Player.Data.States.Grounded;
using Characters.Player.Data.States.Grounded.Moving;
using Data.Player.Moving;
using UnityEngine;

namespace Data.Player
{
    [Serializable]
    public class AliveEntityGroundedData
    {
        [field: SerializeField] [field: Range(0f, 25f)] public float BaseSpeed { get; private set; } = 5f;
        [field: SerializeField] [field: Range(0f, 5f)] public float GroundToFallRayDistance { get; private set; } = 1f;
        [field: SerializeField] public AnimationCurve SlopeSpeedAngles { get; private set; }
        [field: SerializeField] public AliveEntityDashData DashData { get; private set; }
        [field: SerializeField] public AliveEntityWalkData WalkData { get; private set; }
        [field: SerializeField] public AliveEntityRunData RunData { get; private set; }
        [field: SerializeField] public AliveEntityAttackData AttackData { get; private set; }
    }
}
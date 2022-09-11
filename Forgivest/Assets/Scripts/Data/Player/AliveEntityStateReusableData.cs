using Interaction.Core;
using UnityEngine;

namespace Data.Player
{
    public class AliveEntityStateReusableData
    {
        public Vector2 MovementInput { get; set; }

        public float MovementSpeedModifier { get; set; } = 1f;
        public float AttackRate { get; set; }
        public IInteractable InteractableObject { get; set; }
        public float StoppingDistance { get; set; }
        public Vector3 RaycastClickedPoint { get; set; }
    }
}
using Interaction.Core;
using UnityEngine;

namespace Data.Player
{
    public class AliveEntityStateReusableData
    {
        public Vector2 MovementInput { get; set; }

        public float MovementSpeedModifier { get; set; } = 1f;
        public float MovementOnSlopesSpeedModifier { get; set; } = 1f;
        public float MovementDecelerationForce { get; set; } = 1f;
        public bool ShouldWalk { get; set; }
        public bool ShouldSprint { get; set; }
        public bool ShouldMove { get; set; }
        public IInteractable InteractableObject { get; set; }
        public float StoppingDistance { get; set; }

        public Vector3 PositionToMoveTo { get; set; }
        public float SmoothModifier { get; set; }
        public float MaxSmoothModifier { get; set; }

        private Vector3 _currentTargetRotation;
        private Vector3 _timeToReachTargetRotation;
        private Vector3 _dampedTargetRotationCurrentVelocity;
        private Vector3 _dampedTargetRotationPassedTime;

        public ref Vector3 CurrentTargetRotation => ref _currentTargetRotation;
        public ref Vector3 TimeToReachTargetRotation => ref _timeToReachTargetRotation;
        public ref Vector3 DampedTargetRotationCurrentVelocity => ref _dampedTargetRotationCurrentVelocity;
        public ref Vector3 DampedTargetRotationPassedTime => ref _dampedTargetRotationPassedTime;
        public Vector3 LastClickedPoint { get; set; }
    }
}
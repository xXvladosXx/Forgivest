using UnityEngine;

namespace MovementSystem
{
    public class Rotator
    {
        private readonly Rigidbody _rigidbody;

        public Rotator(Rigidbody rigidbody)
        {
            _rigidbody = rigidbody;
        }

        public void RotateRigidbody(Vector3 target, float speed)
        {
            var lookRotation = Quaternion.LookRotation(target - _rigidbody.position);
            
            _rigidbody.MoveRotation(
                Quaternion.Slerp(_rigidbody.transform.rotation,
                    lookRotation, Time.deltaTime*speed));
        }
    }
}
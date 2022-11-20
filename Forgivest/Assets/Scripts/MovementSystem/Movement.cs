using GameCore.Data;
using GameCore.Data.Types;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace MovementSystem
{
    public class Movement : ISavedProgress
    {
        public Transform Transform { get; }

        private readonly NavMeshAgent _navMeshAgent;
        private readonly Rigidbody _rigidbody;

        public Movement(NavMeshAgent navMeshAgent,
            Rigidbody rigidbody,
            Transform transform)
        {
            Transform = transform;
            _navMeshAgent = navMeshAgent;
            _rigidbody = rigidbody;
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(Transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;

            return true;
        }

        public void SetStoppingDistance(float distance)
        {
            _navMeshAgent.stoppingDistance = distance;
        }

        public void MoveTo(Vector3 destination, float speed)
        {
            _navMeshAgent.speed = speed;
            _navMeshAgent.destination = destination;
            _navMeshAgent.isStopped = false;
        }

        public void Stop()
        {
            _navMeshAgent.isStopped = true;
        }

        public float GetDistanceToPoint(Vector3 point) =>
            Vector3.Distance(Transform.position, point);

        public void ResetVelocity() => _rigidbody.velocity = Vector3.zero;

        public Vector3 GetRigidbodyVelocity() => _rigidbody.velocity;
        public Vector3 GetNavMeshVelocity() => _navMeshAgent.velocity;

        public void EnableRotation(bool enable) => _navMeshAgent.updateRotation = enable;
        
        public void UpdateProgress(PlayerProgress playerProgress)
        {
            playerProgress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), Transform.position.AsVector3Data());
            playerProgress.WorldData.RotationOnLevel = new RotationOnLevel(CurrentLevel(), _rigidbody.transform.rotation.AsQuaternionData());
        }

        public void LoadProgress(PlayerProgress playerProgress)
        {
            if (CurrentLevel() != playerProgress.WorldData.PositionOnLevel.Level) return;
            if (CurrentLevel() != playerProgress.WorldData.RotationOnLevel.Level) return;
            
            var savedPosition = playerProgress.WorldData.PositionOnLevel.Position;
            var savedRotation = playerProgress.WorldData.RotationOnLevel.Rotation;
            FindPosition(savedPosition, savedRotation);
        }

        private void FindPosition(Vector3Data savedPosition, QuaternionData quaternionData)
        {
            _navMeshAgent.enabled = false;
            
            Transform.position = savedPosition.AsUnityVector();
            Transform.rotation = quaternionData.AsUnityQuaternion();
            
            _navMeshAgent.enabled = true;
        }

        private string CurrentLevel() => SceneManager.GetActiveScene().name;
    }
}
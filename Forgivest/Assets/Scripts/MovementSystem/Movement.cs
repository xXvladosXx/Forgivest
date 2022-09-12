﻿using UnityEngine;
using UnityEngine.AI;

namespace MovementSystem
{
    public class Movement
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
    }
}
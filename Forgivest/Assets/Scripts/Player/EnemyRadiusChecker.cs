using Player;
using Player.AI.StateMachine;
using UnityEngine;

namespace Installers
{
    public class EnemyRadiusChecker : IEnemyRadiusChecker
    {
        public bool IsEnemiesInRadius(Vector3 position, float radius)
        {
            var colliders = Physics.OverlapSphere(position, radius);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out AIAgent aiAgent))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
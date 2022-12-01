using UnityEngine;

namespace Player
{
    public interface IEnemyRadiusChecker
    {
        bool IsEnemiesInRadius(Vector3 position, float radius);
    }
}
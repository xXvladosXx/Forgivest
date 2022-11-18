using UnityEngine;
using UnityEngine.AI;

namespace CombatSystem.Scripts.Runtime
{
    public class ComponentHeight
    {
        public static Vector3 GetCenterOfCollider(Transform target)
        {
            var collider = target.GetComponent<Collider>();
            var center = collider switch
            {
                CapsuleCollider capsuleCollider => capsuleCollider.center,
                CharacterController characterController => characterController.center,
                _ => Vector3.zero
            };

            return center;
        }
        
        public static float GetComponentHeight(GameObject target)
        {
            float height;

            if (target.TryGetComponent(out NavMeshAgent navMeshAgent))
            {
                height = navMeshAgent.height;
            }else if (target.TryGetComponent(out CharacterController characterController))
            {
                height = characterController.height;
            }
            else if(target.TryGetComponent(out CapsuleCollider capsuleCollider))
            {
                height = capsuleCollider.height;
            }
            else
            {
                height = 0;
            }

            return height;
        }
    }
}
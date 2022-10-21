using System;
using AttackSystem.Core;
using Interaction.Core;
using RaycastSystem.Core;
using UnityEngine;

namespace Enemy
{
    public class EnemyEntity : MonoBehaviour, IInteractable, IDamageReceiver, IRaycastable
    {
        public GameObject GameObject => gameObject;

        public void Interact()
        {
        }

        public event Action<AttackData> OnDamageReceived;
        public LayerMask LayerMask => gameObject.layer;

        public void ReceiverDamage(AttackData attackData)
        {
            print("Damaged enemy");
        }

        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(RaycastUser raycastUser)
        {
            return true;
        }
    }
}
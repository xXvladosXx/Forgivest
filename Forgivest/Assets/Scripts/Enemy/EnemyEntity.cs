using System;
using AttackSystem.Core;
using Interaction.Core;
using UnityEngine;

namespace Enemy
{
    public class EnemyEntity : MonoBehaviour, IInteractable, IDamageReceiver
    {
        public void Interact()
        {
            
        }

        public event Action<AttackData> OnDamageReceived;
        public LayerMask LayerMask => gameObject.layer;
        public void ReceiverDamage(AttackData attackData)
        {
            print("Damaged enemy");

        }
    }
}
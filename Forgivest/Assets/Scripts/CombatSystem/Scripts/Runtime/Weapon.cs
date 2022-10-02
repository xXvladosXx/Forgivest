using System;
using UnityEngine;

namespace CombatSystem.Scripts.Runtime
{
    public class Weapon : MonoBehaviour
    {
        [field: SerializeField] public string ID { get; private set; }

        private void Reset()
        {
            ID = gameObject.name;
        }
    }
}
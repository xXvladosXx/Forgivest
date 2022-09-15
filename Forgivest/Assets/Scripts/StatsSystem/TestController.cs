using System;
using System.Collections.Generic;
using System.Linq;
using AttackSystem.Core;
using StatsSystem.Core;
using StatsSystem.Core.Bonuses;
using StatsSystem.Core.Bonuses.Core;
using UnityEngine;

namespace StatsSystem
{
    public class TestController : MonoBehaviour, IModifier, IDamageReceiver
    {
        [SerializeField] private List<Weapon> _weapon;

        public event Action<List<IBonus>> OnBonusAdded;
        public event Action<List<IBonus>> OnBonusRemoved;

        public List<IBonus> GetBonus()
        {
            var bonuses = new List<IBonus>();

            foreach (var weapon in _weapon)
            {
                bonuses.AddRange(weapon.StatBonus.Cast<IBonus>());
                bonuses.AddRange(weapon.CharacteristicBonus.Cast<IBonus>());
            }

            foreach (var weapon in _weapon)
            {
                print(weapon.StatBonus.Count);
            }

            return bonuses;
        }

        public event Action<AttackData> OnDamageReceived;
        public LayerMask LayerMask => gameObject.layer;
        public void ReceiverDamage(AttackData attackData)
        {
        }
    }

    [Serializable]
    internal class Weapon
    {
        [field: SerializeField] public List<StatBonus> StatBonus { get; private set; }
        [field: SerializeField] public List<CharacteristicBonus> CharacteristicBonus { get; private set; }
    }
}
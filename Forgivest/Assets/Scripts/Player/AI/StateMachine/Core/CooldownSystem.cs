using System.Collections.Generic;
using UnityEngine;

namespace Player.AI.StateMachine.Core
{
    public class CooldownSystem : MonoBehaviour
    {
        public readonly List<CooldownData> _cooldowns = new List<CooldownData>();

        private void Update()
        {
            ProcessCooldowns();
        }
    
        public void AddToCooldown(IHasCooldown cooldown)
        {
            _cooldowns.Add(new CooldownData(cooldown));
        }

        public bool IsOnCooldown(int id)
        {
            foreach (var cooldown in _cooldowns)
            {
                if (cooldown.Id == id)
                {
                    return true;
                }
            }

            return false;
        }
    
        public float GetRemainingDuration(int id)
        {
            foreach (var cooldown in _cooldowns)
            {
                if (cooldown.Id != id)
                {
                    continue;
                }
                return cooldown.RemainingTime;
            }

            return 0f;
        }

        private void ProcessCooldowns()
        { 
            float deltaTime = Time.deltaTime;
            for (int i = _cooldowns.Count-1; i >= 0; i--)
            {
                if (_cooldowns[i].DecrementCooldown(deltaTime))
                {
                    _cooldowns.RemoveAt(i);
                }
            }
        }
    }

    public class CooldownData
    {
        public CooldownData(IHasCooldown cooldown)
        {
            Id = cooldown.Id;
            RemainingTime = cooldown.CooldownDuration;
        }
    
        public int Id { get; private set; }
        public float RemainingTime { get; private set; }
    
        public bool DecrementCooldown(float deltaTime)
        {
            RemainingTime = Mathf.Max(RemainingTime - deltaTime,0f);
            return RemainingTime == 0f;
        }
    }
}
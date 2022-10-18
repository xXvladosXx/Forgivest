using System;
using StatSystem;
using UI.HUD.Stats;
using Zenject;

namespace Controllers.Player
{
    public class PlayerHealthController : IInitializable, ITickable, IDisposable
    {
        private readonly Health _health;
        private readonly HealthBarUI _healthBarUI;

        public PlayerHealthController(Health health, HealthBarUI healthBarUI)
        {
            _health = health;
            _healthBarUI = healthBarUI;
        }
        
        public void Initialize()
        {
            _health.OnCurrentValueChanged += RecalculateHealthOnBar; 
        }

        private void RecalculateHealthOnBar()
        {
            _healthBarUI.RecalculateHealth(_health.value);
        }

        public void Tick()
        {
        }

        public void Dispose()
        {
        }
    }
}
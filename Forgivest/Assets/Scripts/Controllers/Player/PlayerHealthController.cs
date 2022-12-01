using System;
using StatsSystem.Scripts.Runtime;
using StatSystem;
using UI.HUD.Stats;
using Zenject;

namespace Controllers.Player
{
    public class PlayerHealthController : IInitializable, IDisposable
    {
        private readonly StatController _statController;
        private readonly HealthBarUI _healthBarUI;
        private Health _health;

        public PlayerHealthController(StatController statController, HealthBarUI healthBarUI)
        {
            _statController = statController;
            _healthBarUI = healthBarUI;
        }
        
        public void Initialize()
        {
            _health = _statController.Health;
            _health.OnCurrentValueChanged += RecalculateHealthOnBar;
            _health.OnValueChanged += RecalculateHealthOnBar;
            
            RecalculateHealthOnBar(_health.CurrentValue, _health.Value);
        }

        public void Dispose()
        {
            _health.OnCurrentValueChanged -= RecalculateHealthOnBar;
            _health.OnValueChanged -= RecalculateHealthOnBar;
        }
        
        private void RecalculateHealthOnBar(float maxHealth)
        {
            _healthBarUI.RecalculateHealth(maxHealth);
        }
        
        private void RecalculateHealthOnBar(float currentHealth, float maxHealth)
        {
            _healthBarUI.RecalculateHealth(currentHealth, maxHealth);
        }
    }
}
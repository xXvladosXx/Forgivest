﻿using System;
using StatsSystem.Scripts.Runtime;
using StatSystem;
using UI.HUD.Stats;
using Zenject;
using Attribute = StatSystem.Attribute;

namespace Controllers.Player
{
    public class PlayerManaController : IInitializable, ITickable, IDisposable
    {
        private readonly StatController _statController;
        private readonly ManaBarUI _manaBarUI;
        private Attribute _mana;

        public PlayerManaController(StatController statController, ManaBarUI manaBarUI)
        {
            _statController = statController;
            _manaBarUI = manaBarUI;
        }

        public void Initialize()
        {
            _mana = _statController.Stats["Mana"] as Attribute;

            _mana.OnCurrentValueChanged += RecalculateManaOnBar;
            _mana.OnValueChanged += RecalculateManaOnBar;

            RecalculateManaOnBar(_mana.currentValue, _mana.Value);
        }

        public void Tick()
        {
        }

        public void Dispose()
        {
            _mana.OnCurrentValueChanged -= RecalculateManaOnBar;
            _mana.OnValueChanged -= RecalculateManaOnBar;
        }

        private void RecalculateManaOnBar(float currentHealth, float maxHealth)
        {
            _manaBarUI.RecalculateMana(currentHealth, maxHealth);
        }

        private void RecalculateManaOnBar(float maxValue)
        {
            _manaBarUI.RecalculateMana(maxValue);
        }
    }
}
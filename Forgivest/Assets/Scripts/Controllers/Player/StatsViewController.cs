using System;
using System.Text;
using System.Text.RegularExpressions;
using StatSystem;
using UI.Inventory.Core;
using Zenject;
using Attribute = StatSystem.Attribute;

namespace Controllers.Player
{
    public class StatsViewController : IInitializable, IDisposable
    {
        private readonly InventoryPanel _inventoryPanel;
        private readonly StatController _statsController;

        public StatsViewController(InventoryPanel inventoryPanel, StatController statsController)
        {
            _inventoryPanel = inventoryPanel;
            _statsController = statsController;
        }
        
        public void Initialize()
        {
            OnStatChanged();
            
            _statsController.OnStatsChanged += OnStatChanged;
        }

        public void Dispose()
        {
            _statsController.OnStatsChanged -= OnStatChanged;
        }

        private void OnStatChanged()
        {
            var primaryStats = new StringBuilder();
            var attributes = new StringBuilder();
            
            foreach (var stat in _statsController.Stats)
            {
                if (stat.Value is PrimaryStat primaryStat)
                {
                    primaryStats.AppendLine($"{($"{Regex.Replace(stat.Key, "([a-z])([A-Z])", "$1 $2")}")}: {primaryStat.Value}");
                }
                else
                {
                    attributes.AppendLine($"{($"{Regex.Replace(stat.Key, "([a-z])([A-Z])", "$1 $2")}")}: {stat.Value.Value}");
                }
            }
            
            _inventoryPanel.StatsView.SetStats(primaryStats.ToString(), attributes.ToString());
        }
    }
}
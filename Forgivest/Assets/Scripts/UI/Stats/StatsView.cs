using TMPro;
using UI.Core;
using UnityEngine;

namespace UI.Stats
{
    public class StatsView : UIElement
    {
        [SerializeField] private TextMeshProUGUI _primaryStats;
        [SerializeField] private TextMeshProUGUI _attributes;
        [SerializeField] private TextMeshProUGUI _primaryStatsValues;
        [SerializeField] private TextMeshProUGUI _attributesValues;
        
        public void SetStats(string primaryStats, string attributes, string primaryStatsValues, string attributesValues)
        {
            _primaryStats.text = primaryStats;
            _attributes.text = attributes;
            _primaryStatsValues.text = primaryStatsValues;
            _attributesValues.text = attributesValues;
        }

        public void SetStats(string primaryStats, string attributes)
        {
            _primaryStats.text = primaryStats;
            _attributes.text = attributes;
        }
    }
}
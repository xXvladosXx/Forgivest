using Core;
using UnityEngine;

namespace StatsSystem.Scripts.Runtime
{
    [CreateAssetMenu(fileName = "StatDefinition", menuName = "StatSystem/StatDefinition", order = 0)]
    public class StatDefinition : ScriptableObject
    {
        [SerializeField] private int m_BaseValue;
        [SerializeField] private int m_Cap = -1;
        [SerializeField] private NodeGraph m_Formula;
        
        public int BaseValue => m_BaseValue;
        public int Cap => m_Cap;
        public NodeGraph Formula => m_Formula;

    }
}
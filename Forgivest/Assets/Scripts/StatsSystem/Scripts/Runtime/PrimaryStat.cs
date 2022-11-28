using System;
using System.Runtime.CompilerServices;
using SaveSystem.Scripts.Runtime;
using UnityEngine;

[assembly: InternalsVisibleTo("StatSystem.Tests")]
namespace StatsSystem.Scripts.Runtime
{
    public class PrimaryStat : Stat, ISavable
    {
        [field: SerializeField] public SaveDataChannel SaveDataChannel { get; }

        private float m_BaseValue;
        public override float baseValue => m_BaseValue;
        
        public PrimaryStat(StatDefinition definition, StatController controller) : base(definition, controller)
        {
        }
        
        public override void Initialize()
        {
            m_BaseValue = Definition.BaseValue;
            base.Initialize();
        }

        internal void Add(int amount)
        {
            m_BaseValue += amount;
            CalculateOnValue(0);
        }

        internal void Subtract(int amount)
        {
            m_BaseValue -= amount;
            CalculateOnValue(0);
        }

        #region Stat System


        public object CaptureState => new PrimaryStatData
        {
            baseValue = baseValue
        };
        public void RestoreState(object data)
        {
            PrimaryStatData primaryStatData = (PrimaryStatData)data;
            m_BaseValue = primaryStatData.baseValue;
            CalculateOnValue(0);
        }

        [Serializable]
        protected class PrimaryStatData
        {
            public float baseValue;
        }

        #endregion
        
    }
}
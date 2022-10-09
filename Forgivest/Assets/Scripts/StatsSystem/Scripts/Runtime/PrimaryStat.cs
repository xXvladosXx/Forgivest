using System;
using System.Runtime.CompilerServices;
using SaveSystem.Scripts.Runtime;

[assembly: InternalsVisibleTo("StatSystem.Tests")]
namespace StatSystem
{
    public class PrimaryStat : Stat, ISavable
    {
        private float m_BaseValue;
        public override float baseValue => m_BaseValue;
        
        public PrimaryStat(StatDefinition definition, StatController controller) : base(definition, controller)
        {
        }
        
        public override void Initialize()
        {
            m_BaseValue = definition.BaseValue;
            base.Initialize();
        }

        internal void Add(int amount)
        {
            m_BaseValue += amount;
            CalculateValue();
        }

        internal void Subtract(int amount)
        {
            m_BaseValue -= amount;
            CalculateValue();
        }

        #region Stat System

        public object Data => new PrimaryStatData
        {
            baseValue = baseValue
        };
        public void Load(object data)
        {
            PrimaryStatData primaryStatData = (PrimaryStatData)data;
            m_BaseValue = primaryStatData.baseValue;
            CalculateValue();
        }

        [Serializable]
        protected class PrimaryStatData
        {
            public float baseValue;
        }

        #endregion
        
    }
}
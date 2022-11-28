using System;
using Core;
using UnityEngine;

namespace StatsSystem.Scripts.Runtime
{
    public class Health : Attribute
    {
        private readonly TagRegister _tagRegister;

        public Health(StatDefinition definition, StatController controller, TagRegister tagRegister) : base(definition, controller)
        {
            _tagRegister = tagRegister;
        }

        public override void ApplyModifier(StatModifier modifier)
        {
            ITaggable source = modifier.Source as ITaggable;

            if(_tagRegister != null)
            {if (_tagRegister.Contains("zombify"))
            {
                if (source.Tags.Contains("healing"))
                {
                    modifier.Magnitude *= -1;
                }
            }}
            
            if (source != null)
            {
                if (source.Tags.Contains("physical"))
                {
                    var possibleDamage = FindResistance(modifier, "PhysicalDefense");
                    modifier.Magnitude = possibleDamage;
                }
                else if (source.Tags.Contains("magical"))
                {
                    var possibleDamage = FindResistance(modifier, "MagicalDefense");
                    modifier.Magnitude = possibleDamage;
                }
            }
            
            base.ApplyModifier(modifier);
        }

        private float FindResistance(StatModifier modifier, string stat)
        {
            float resistance = Mathf.Clamp(m_Controller.Stats[stat].Value, 1, 100);
            float possibleDamage = modifier.Magnitude;

            resistance = (resistance * possibleDamage) / 100;
            resistance = (float) Math.Round(resistance, 2); 
            possibleDamage -= resistance;
            return possibleDamage;
        }
    }
}
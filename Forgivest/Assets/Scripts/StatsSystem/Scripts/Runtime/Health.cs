using System;
using AbilitySystem.AbilitySystem.Runtime;
using Core;
using StatsSystem.Scripts.Runtime;
using UnityEngine;

namespace StatSystem
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

                    possibleDamage -= modifier.Magnitude;
                    modifier.Magnitude = possibleDamage;
                }
                else if (source.Tags.Contains("magical"))
                {
                    var possibleDamage = FindResistance(modifier, "MagicalDefense");
                    
                        possibleDamage -= modifier.Magnitude;
                        modifier.Magnitude = possibleDamage;
                }
                else if (source.Tags.Contains("pure"))
                {
                    // do nothing
                }
            }
            
            base.ApplyModifier(modifier);
        }

        private float FindResistance(StatModifier modifier, string stat)
        {
            float resistance = Mathf.Clamp(m_Controller.Stats[stat].Value, 1, 100);
            var possibleDamage = modifier.Magnitude;
            possibleDamage =  (possibleDamage * 100) / resistance;
            return possibleDamage;
        }
    }
}
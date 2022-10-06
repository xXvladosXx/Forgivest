using AbilitySystem.AbilitySystem.Runtime;
using Core;
using UnityEngine;

namespace StatSystem
{
    public class Health : Attribute
    {
        private readonly TagController _tagController;

        public Health(StatDefinition definition, StatController controller, TagController tagController) : base(definition, controller)
        {
            _tagController = tagController;
        }

        public override void ApplyModifier(StatModifier modifier)
        {
            ITaggable source = modifier.Source as ITaggable;

            if(_tagController != null)
            {if (_tagController.Contains("zombify"))
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
            float resistance = Mathf.Clamp(m_Controller.Stats[stat].value, 1, 100);
            var possibleDamage = modifier.Magnitude;
            possibleDamage =  (possibleDamage * 100) / resistance;
            return possibleDamage;
        }
    }
}
using AbilitySystem.AbilitySystem.Runtime;
using Core;

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
            ITaggable source = modifier.source as ITaggable;

            if (_tagController.Contains("zombify"))
            {
                if (source.Tags.Contains("healing"))
                {
                    modifier.magnitude *= -1;
                }
            }
            
            if (source != null)
            {
                if (source.Tags.Contains("physical"))
                {
                    modifier.magnitude += m_Controller.stats["PhysicalDefense"].value;
                }
                else if (source.Tags.Contains("magical"))
                {
                    modifier.magnitude += m_Controller.stats["MagicalDefense"].value;
                }
                else if (source.Tags.Contains("pure"))
                {
                    // do nothing
                }
            }
            
            base.ApplyModifier(modifier);
        }
    }
}
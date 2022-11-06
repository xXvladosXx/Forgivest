using AbilitySystem.AbilitySystem.Runtime.Abilities;
using InventorySystem.Interaction;
using LevelSystem;

namespace Requirements.Core
{
    public interface IRequirementUser
    {
        LevelController LevelController { get; }
        AbilityHandler AbilityHandler { get; }
        ObjectPicker ObjectPicker { get; }
    }
}
using AbilitySystem.AbilitySystem.Runtime.Abilities;
using InventorySystem.Interaction;
using LevelSystem;
using LevelSystem.Scripts.Runtime;

namespace Requirements.Core
{
    public interface IRequirementUser
    {
        LevelController LevelController { get; }
        AbilityHandler AbilityHandler { get; }
        ObjectPicker ObjectPicker { get; }
    }
}
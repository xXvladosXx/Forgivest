using UnityEngine;

namespace InventorySystem.Items
{
    public abstract class Requirement : ScriptableObject
    {
        public abstract bool CheckRequirements();
    }
}
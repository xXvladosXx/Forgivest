using UnityEngine;

namespace Requirements.Core
{
    public abstract class Requirement : ScriptableObject
    {
        public abstract bool CheckRequirements();
    }
}
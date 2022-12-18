using System;

namespace GameCore.SaveSystem.Data.Types
{
    [Serializable]
    public class WasDestroyedOnLevel
    {
        public string Level; 
        public bool WasDestroyed;
        
        public WasDestroyedOnLevel(string level)
        {
            Level = level;
        }
        
        public WasDestroyedOnLevel(string level, bool wasDestroyed)
        {
            Level = level;
            WasDestroyed = wasDestroyed;
        }
    }
}
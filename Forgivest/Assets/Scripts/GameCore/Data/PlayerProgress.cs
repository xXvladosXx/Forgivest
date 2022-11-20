using System;

namespace GameCore.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        
        public PlayerProgress(string scene)
        {
            WorldData = new WorldData(scene);
        }
    }
}
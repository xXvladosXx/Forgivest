using System;

namespace GameCore.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public string Scene;

        public PlayerProgress(string scene)
        {
            WorldData = new WorldData(scene);
            Scene = scene;  
        }
    }
}
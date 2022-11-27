using System;
using GameCore.SaveSystem.Data.Types;

namespace GameCore.SaveSystem.Data
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
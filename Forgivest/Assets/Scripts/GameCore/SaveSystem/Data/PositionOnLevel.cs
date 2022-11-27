using System;
using GameCore.SaveSystem.Data.Types;

namespace GameCore.SaveSystem.Data
{
    [Serializable]
    public class PositionOnLevel
    {
        public string Level;
        public Vector3Data Position;
        public PositionOnLevel(string level, Vector3Data vector3Data)
        {
            Level = level;
            Position = vector3Data;
        }

        public PositionOnLevel(string level)
        {
            Level = level;
        }
    }
}
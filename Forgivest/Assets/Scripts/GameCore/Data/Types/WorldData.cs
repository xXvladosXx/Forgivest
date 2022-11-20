﻿using System;

namespace GameCore.Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public RotationOnLevel RotationOnLevel;

        public WorldData(string level)
        {
            PositionOnLevel = new PositionOnLevel(level);
            RotationOnLevel = new RotationOnLevel(level);
        }

    }
}
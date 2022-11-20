﻿using System;
using GameCore.Data.Types;

namespace GameCore.Data
{
    [Serializable]
    public class RotationOnLevel
    {
        public QuaternionData Rotation;
        public string Level;

        public RotationOnLevel(string level)
        {
            Level = level;
        }
        
        public RotationOnLevel(string level, QuaternionData quaternionData)
        {
            Level = level;
            Rotation = quaternionData;
        }
    }
}
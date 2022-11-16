﻿namespace SaveSystem.Scripts.Runtime
{
    public interface ISavable
    {
        object Data { get; }
        void Load(object data);
    }
}
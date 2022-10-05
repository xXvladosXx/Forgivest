using System;

namespace LevelSystem
{
    public interface ILevelable
    {
        int Level { get; }
        event Action OnLevelChanged;
        event Action OnCurrentExperienceChanged;
        int CurrentExperience { get; set; }
        int RequiredExperience { get; }
        bool IsInitialized { get; }
        event Action OnInitialized;
        event Action OnWillUninitialize;
    }
}
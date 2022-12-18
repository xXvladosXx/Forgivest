using System;

namespace LevelSystem.Scripts.Runtime
{
    public interface ILevelable
    {
        int Level { get; }
        event Action OnLevelChanged;
        event Action<int, int, int> OnCurrentExperienceChanged;
        int CurrentExperience { get; set; }
        int RequiredExperience { get; }
        bool IsInitialized { get; }
        event Action OnInitialized;
        event Action OnWillUninitialize;
    }
}
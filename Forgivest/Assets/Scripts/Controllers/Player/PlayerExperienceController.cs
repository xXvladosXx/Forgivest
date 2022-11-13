using System;
using LevelSystem;
using UI.HUD.Stats;
using Zenject;

namespace Controllers.Player
{
    public class PlayerExperienceController : IInitializable, IDisposable
    {
        private readonly ExperienceBarUI _experienceBarUI;
        private readonly LevelController _levelController;
        

        public PlayerExperienceController(ExperienceBarUI experienceBarUI, LevelController levelController)
        {
            _experienceBarUI = experienceBarUI;
            _levelController = levelController;
        }
        
        public void Initialize()
        {
            _levelController.OnCurrentExperienceChanged += RecalculateExperienceBar;
            
            RecalculateExperienceBar(_levelController.CurrentExperience, _levelController.RequiredExperience);
        }

        private void RecalculateExperienceBar(int experience, int maxExperience)
        {
            _experienceBarUI.RecalculateExperience(experience, maxExperience);
        }

        public void Dispose()
        {
            _levelController.OnCurrentExperienceChanged -= RecalculateExperienceBar;
        }
    }
}
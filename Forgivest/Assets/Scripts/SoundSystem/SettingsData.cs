namespace SoundSystem
{
    public struct SettingsData
    {
        public float MusicVolume { get; }
        public float EffectsVolume { get; }
        public int ResolutionIndex { get; }
        public int IsFullScreen { get; }
        public int QualityLevel { get; }

        public SettingsData(float musicVolume, 
            float effectsVolume, int qualityLevel, 
            int isFullScreen, int resolutionIndex)
        {
            MusicVolume = musicVolume;
            EffectsVolume = effectsVolume;
            QualityLevel = qualityLevel;
            IsFullScreen = isFullScreen;
            ResolutionIndex = resolutionIndex;
        }
    }
}
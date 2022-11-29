namespace SoundSystem
{
    public class AudioSettingsData
    {
        public AudioSettingsData(float musicVolume, float effectsVolume)
        {
            MusicVolume = musicVolume;
            EffectsVolume = effectsVolume;
        }

        public float MusicVolume { get; }
        public float EffectsVolume { get; }
    }
}
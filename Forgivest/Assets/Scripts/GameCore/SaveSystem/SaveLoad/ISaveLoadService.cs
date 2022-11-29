using GameCore.SaveSystem.Data;
using SoundSystem;


namespace GameCore.SaveSystem.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveProgress(string saveFile);
        PlayerProgress Load(string saveFile);
        AudioSettingsData LoadAudioSettings();
        void SaveAudioSettings(AudioSettingsData audioSettingsData);
    }
}
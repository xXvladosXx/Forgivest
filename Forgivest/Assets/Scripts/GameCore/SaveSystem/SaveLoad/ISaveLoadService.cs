using GameCore.SaveSystem.Data;
using SoundSystem;

namespace GameCore.SaveSystem.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveProgress(string saveFile);
        PlayerProgress Load(string saveFile);
        SettingsData LoadSettings();
        void SaveGraphicsSettings(int resolution, int isFullscreen, int graphics);
        void SaveAudioSettings(float musicVolume, float effectsVolume);
    }
}
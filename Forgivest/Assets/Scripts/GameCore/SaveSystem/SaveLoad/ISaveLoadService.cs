using GameCore.SaveSystem.Data;

namespace GameCore.SaveSystem.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveProgress(string saveFile);
        PlayerProgress Load(string saveFile);
    }
}
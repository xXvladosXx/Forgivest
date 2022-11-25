namespace GameCore.Data.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveProgress(string saveFile);
        PlayerProgress Load(string saveFile);
    }
}
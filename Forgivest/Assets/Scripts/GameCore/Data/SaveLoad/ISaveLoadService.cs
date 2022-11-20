namespace GameCore.Data.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveProgress();
        PlayerProgress Load();
    }
}
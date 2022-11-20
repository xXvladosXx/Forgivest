namespace GameCore.SaveSystem.Scripts.Runtime
{
    public interface ISavable
    {
        SaveDataChannel SaveDataChannel { get; }
        object CaptureState { get; }
        void RestoreState(object data);
    }
}
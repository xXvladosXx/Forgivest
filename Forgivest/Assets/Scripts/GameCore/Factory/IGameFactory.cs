using System.Collections.Generic;
using GameCore.Crutches;
using GameCore.SaveSystem.Data;
using UnityEngine;

namespace GameCore.Factory
{
    public interface IGameFactory
    {
        GameObject CreatePlayer(GameObject at);
        IPlayerObserver PlayerObserver { get; }
        IUIObserver UIObserver { get; }
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        void CleanUp();
        void Register(ISavedProgressReader progressReader);
    }
}
using System.Collections.Generic;
using GameCore.Crutches;
using GameCore.SaveSystem.Data;
using UI.Menu;
using UnityEngine;

namespace GameCore.Factory
{
    public interface IGameFactory
    {
        GameObject CreatePlayer(GameObject at);
        IPlayerObserver PlayerObserver { get; }
        IUIObserver UIObserver { get; }
        LoadMenu LoadMenu { get; set; }
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        void CleanUp();
        void Register(ISavedProgressReader progressReader);
    }
}
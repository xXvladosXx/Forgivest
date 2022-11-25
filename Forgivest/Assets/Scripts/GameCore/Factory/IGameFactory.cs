using System.Collections.Generic;
using GameCore.Data;
using GameCore.StateMachine;
using UnityEngine;

namespace GameCore.Factory
{
    public interface IGameFactory
    {
        GameObject CreatePlayer(GameObject at);
        IPlayerObserver PlayerObserver { get; }
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        void CleanUp();
        void Register(ISavedProgressReader progressReader);
    }
}
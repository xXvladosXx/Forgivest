using UnityEngine;

namespace GameCore.AssetManagement
{
    public interface IAssetProvider
    {
        GameObject Instantiate(string path, Vector3 at);
        GameObject Instantiate(string path);
        T Instantiate<T>(string path) where T : Component;
    }
}
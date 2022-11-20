using UnityEngine;

namespace GameCore.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        public GameObject Instantiate(string path, Vector3 at)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }

        public T Instantiate<T>(string path) where T : Component
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab).GetComponent<T>();
        }

        public GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }
    }
}
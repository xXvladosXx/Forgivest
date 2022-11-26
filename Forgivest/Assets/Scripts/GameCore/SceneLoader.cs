using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace GameCore
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string name, string saveFile, Action<string> onLoaded = null)
        {
            _coroutineRunner.StartCoroutine(LoadScene(name, saveFile, onLoaded));
        }
        
        public IEnumerator LoadScene(string sceneName, string saveFile, Action<string> onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                onLoaded?.Invoke(saveFile);
                yield break;
            }
            
            var waitForSceneLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!waitForSceneLoad.isDone)
            {
                yield return null;
            }
                
            onLoaded?.Invoke(saveFile);
        }
    }
}
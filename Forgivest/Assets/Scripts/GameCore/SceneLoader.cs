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

        public void Load(string name,Action onLoaded = null)
        {
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));
        }
        
        public IEnumerator LoadScene(string sceneName, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                yield break;
            }
            
            var waitForSceneLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!waitForSceneLoad.isDone)
            {
                yield return null;
            }
                
            onLoaded?.Invoke();
        }
    }
}
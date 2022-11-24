using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore
{
    public class GameBootstrapperFinder : MonoBehaviour
    {
        private void Start()
        {
            if (GameObject.FindObjectOfType<GameBootstrapper>() == null)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
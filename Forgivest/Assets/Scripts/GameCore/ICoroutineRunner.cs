using System.Collections;
using UnityEngine;

namespace GameCore
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator routine);
    }
}
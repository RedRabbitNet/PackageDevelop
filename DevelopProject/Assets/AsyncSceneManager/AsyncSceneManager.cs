using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 非同期のシーンロードをサポートするクラス
/// </summary>
public class AsyncSceneManager : Singleton<AsyncSceneManager>
{
    public void LoadScene(string sceneName, Action callback = null)
    {
        StartCoroutine(loadSceneCoroutine(sceneName, callback));
    }

    private IEnumerator loadSceneCoroutine(string sceneName, Action callback = null)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
        callback?.Invoke();
    }
}

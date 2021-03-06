﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アプリ固有のシーン遷移挙動を定義するクラス
/// </summary>
public class AppSceneManager : Singleton<AppSceneManager>
{
    public void LoadScene(string sceneName)
    {
        Debug.Log("AppSceneManager:LoadScene");
        StartCoroutine(loadSceneCoroutine(sceneName));
    }

    private IEnumerator loadSceneCoroutine(string sceneName)
    {
        Coroutine beforeCoroutine = StartCoroutine(TransitionShaderManager.Instance.beforeTransitionCoroutine());
        yield return StartCoroutine(SceneManagerExtend.Instance.LoadSceneAsyncCoroutine(sceneName, beforeCoroutine));
        StartCoroutine(TransitionShaderManager.Instance.afterTransitionCoroutine());
    }

    public void Initialize()
    {
        
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     popup.Cloase();
        // }

        if (Input.GetKeyDown(KeyCode.A))
        {
            LoadScene("SampleScene2");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            LoadScene("SampleScene");
        }
    }
}

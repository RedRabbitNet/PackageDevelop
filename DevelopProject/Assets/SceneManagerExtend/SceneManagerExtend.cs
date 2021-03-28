using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// SceneManagerの拡張クラス
/// </summary>
public class SceneManagerExtend : Singleton<SceneManagerExtend>
{
    /// <summary>
    /// シングルトン初期化後の処理 - 初期化
    /// </summary>
    protected override void onInitialized()
    {
        //イベント関数の登録
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SceneManager.sceneLoaded        += OnSceneLoaded;
        SceneManager.sceneUnloaded      += OnSceneUnloaded;
    }
    
    
    //------------------------------------------------------------------------
    //イベント関数
    
    /// <summary>
    /// アクティブシーン変更時
    /// </summary>
    private void OnActiveSceneChanged( Scene i_preChangedScene, Scene i_postChangedScene )
    {
        // Debug.LogFormat( "OnActiveSceneChanged() preChangedScene:{0} postChangedScene:{1}", i_preChangedScene.name, i_postChangedScene.name );
    }

    /// <summary>
    /// シーン読み込み時
    /// </summary>
    private void OnSceneLoaded( Scene i_loadedScene, LoadSceneMode i_mode )
    {
        // Debug.LogFormat( "OnSceneLoaded() current:{0} loadedScene:{1} mode:{2}", SceneManager.GetActiveScene().name, i_loadedScene.name, i_mode );
    }

    /// <summary>
    /// シーン破棄時
    /// </summary>
    private void OnSceneUnloaded( Scene i_unloadedScene )
    {
        // Debug.LogFormat( "OnSceneUnloaded() current:{0} unloaded:{1}", SceneManager.GetActiveScene().name, i_unloadedScene.name );
    }
    
    //------------------------------------------------------------------------
    //外部呼出し関数
    
    /// <summary>
    /// シーンの同期読み込み
    /// </summary>
    public void LoadScene(string sceneName, Action activateCallback = null)
    {
        SceneManager.LoadScene(sceneName);
        activateCallback?.Invoke();
    }

    /// <summary>
    /// シーンの非同期読み込み
    /// コルーチンが終わるのを待って移行する
    /// </summary>
    public IEnumerator LoadSceneAsyncCoroutine(string sceneName, Coroutine duringCoroutine, Action activateCallback = null)
    {
        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(sceneName);
        loadSceneAsync.allowSceneActivation = false;                //ロード完了後に自動で移行しない設定
        while (!loadSceneAsync.isDone)
        {
            //ロードが完了していた場合
            if (loadSceneAsync.progress >= 0.9f)
            {
                //コルーチンが終わるのを待ってシーンを移行する
                yield return duringCoroutine;
                loadSceneAsync.allowSceneActivation = true;
                activateCallback?.Invoke();
            }

            yield return null;
        }
    }
    

    /// <summary>
    /// シーンの非同期読み込み
    /// ロード完了時に条件を待って移行する
    /// </summary>
    public IEnumerator LoadSceneAsyncCoroutine(string sceneName, Func<bool> waitTrueFunction, Action activateCallback = null)
    {
        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(sceneName);
        loadSceneAsync.allowSceneActivation = false;                //ロード完了後に自動で移行しない設定
        while (!loadSceneAsync.isDone)
        {
            //ロードが完了していた場合
            if (loadSceneAsync.progress >= 0.9f)
            {
                //何らかの条件を満たしている場合にシーンを移行する
                if (waitTrueFunction())
                {
                    loadSceneAsync.allowSceneActivation = true;
                    activateCallback?.Invoke();
                }
            }

            yield return null;
        }
    }
}

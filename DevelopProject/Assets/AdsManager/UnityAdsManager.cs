using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

/// <summary>
/// UnityAdsを使った広告管理
/// </summary>
public class UnityAdsManager : Singleton<UnityAdsManager>,　IUnityAdsListener 
{
    private bool isAdsStart;
    private bool isAdsFinish;
    
    public IEnumerator InitializeCoprotuine(string gameId, bool testMode = true)
    {
        Advertisement.AddListener (this);
        Advertisement.Initialize (gameId, testMode);

        yield return null;
    }
    
    public void OnDestroy() {
        Advertisement.RemoveListener(this);
    }

    /// <summary>
    /// 動画広告再生関連のフラグの初期化
    /// </summary>
    private void RewardVideoFlgInitialize()
    {
        isAdsStart = false;
        isAdsFinish = false;
    }
    
    /// <summary>
    /// 動画広告の再生
    /// </summary>
    public IEnumerator ShowRewardedVideoCoroutine(string surfacingId, Action callback = null)
    {
        if (Advertisement.IsReady(surfacingId))
        {
            RewardVideoFlgInitialize();
            Advertisement.Show(surfacingId);
        } 
        else
        {
            Debug.LogWarning("Rewarded video is not ready at the moment! Please try again later!");
        }

        while (!isAdsFinish)
        {
            yield return null;
        }
        
        callback?.Invoke();
    }

    /// <summary>
    /// 動画広告の準備完了
    /// </summary>
    public void OnUnityAdsReady(string placementId)
    {
    }

    /// <summary>
    /// 動画再生エラー
    /// </summary>
    public void OnUnityAdsDidError(string message)
    {
        Debug.LogWarning("OnUnityAdsDidError:" + message);
    }

    /// <summary>
    /// 動画再生開始
    /// </summary>
    public void OnUnityAdsDidStart(string placementId)
    {
        isAdsStart = true;
        isAdsFinish = false;
    }

    /// <summary>
    /// 動画再生終了
    /// </summary>
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        isAdsStart = false;   
        isAdsFinish = true;
    }
}

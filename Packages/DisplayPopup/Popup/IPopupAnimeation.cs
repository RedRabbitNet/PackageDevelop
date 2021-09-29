using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// ポップアップのアニメーション関連インターフェース
/// </summary>
public interface IPopupAnimeation
{
    /// <summary>
    /// 初期化処理
    /// </summary>
    void InitializeAnimation();
    
    /// <summary>
    /// 開く処理
    /// </summary>
    void Open();

    /// <summary>
    /// 閉じる処理
    /// </summary>
    void Close();
}

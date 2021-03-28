using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


/// <summary>
/// 継承して使用するポップアップクラス
/// </summary>
public abstract class AbstractPopup : MonoBehaviour
{
    protected Sequence openTween;
    protected Sequence closeTween;
    
    /// <summary>
    /// 初期化処理
    /// </summary>
    public virtual void Initialize()
    {
        SetOpenTween();
        SetCloseTween();
    }

    /// <summary>
    /// 開く処理
    /// </summary>
    public virtual void Open()
    {
        openTween.Restart();
    }

    /// <summary>
    /// 閉じる処理
    /// </summary>
    public virtual void Close()
    {
        closeTween.Restart();
    }

    /// <summary>
    /// 開く際のアニメーションの設定
    /// </summary>
    protected virtual void SetOpenTween()
    {
        openTween
            .Pause()
            .SetAutoKill(false)
            .SetLink(gameObject);
    }

    /// <summary>
    /// 閉じる際のアニメーションの設定
    /// </summary>
    protected virtual void SetCloseTween()
    {
        closeTween
            .Pause()
            .SetAutoKill(false)
            .SetLink(gameObject);
    }
}

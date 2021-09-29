using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


/// <summary>
/// 継承して使用するポップアップクラス
/// </summary>
public abstract class AbstractPopup : MonoBehaviour, IPopupAnimeation, IDisplay
{
    protected Sequence openTween;
    protected Sequence closeTween;
    
    /// <summary>
    /// 初期化処理
    /// </summary>
    public virtual void InitializeAnimation()
    {
        openTween = DOTween.Sequence();
        closeTween = DOTween.Sequence();
        SetOpenTween();
        SetCloseTween();
    }

    /// <summary>
    /// 開く処理
    /// </summary>
    public virtual void Open()
    {
        if (openTween == null)
        {
            Debug.LogWarning("InitializeAnimationが呼ばれていない可能性があります");
        }
        
        
        openTween.Restart();
    }

    /// <summary>
    /// 閉じる処理
    /// </summary>
    public virtual void Close()
    {
        if (closeTween == null)
        {
            Debug.LogWarning("InitializeAnimationが呼ばれていない可能性があります");
        }
        
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
    
    public abstract void initializeView();
    public abstract void updateView();
}
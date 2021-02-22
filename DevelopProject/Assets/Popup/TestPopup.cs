using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TestPopup : AbstractPopup
{
    [SerializeField] private Image backgroundImage;

    public override void Initialize()
    {
        base.Initialize();
        
        backgroundImage.rectTransform.localScale = Vector3.zero;
    }

    protected override void SetCloseTween()
    {
        closeTween = DOTween.Sequence();

        closeTween.Append(backgroundImage.rectTransform.DOScale(Vector3.zero, 1.0f));
        
        base.SetCloseTween();
    }

    protected override void SetOpenTween()
    {
        openTween = DOTween.Sequence();

        openTween.Append(backgroundImage.rectTransform.DOScale(Vector3.one, 1.0f));

        base.SetOpenTween();
    }
}

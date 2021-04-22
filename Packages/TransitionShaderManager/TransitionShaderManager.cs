using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shaderによる遷移エフェクトを管理するクラス
/// シーン中のCanvasはScreenSpace-Cameraに設定されており、メインカメラと紐づくことを前提としている
/// </summary>
public class TransitionShaderManager : Singleton<TransitionShaderManager>
{
    private TransitionRenderCanvas transitionCanvasPrefab;
    private TransitionRenderCanvas transitionCanvas;
    
    private RenderTexture renderTexture;
    private Material transitionMaterial;
    
    private float transitionTimeCount;
    private float transitionTime;       //After,Before Transitionにかかる時間 前後の2つで1セットの遷移であるため倍の時間がかかることに注意
    private bool transitionEnd;
    
    
    public void Initialize(Material setMaterial, float setTransitionTime = 0.5f)
    {
        transitionMaterial = setMaterial;
        transitionTime = setTransitionTime;

        transitionCanvasPrefab = Resources.Load<TransitionRenderCanvas>("TransitionRenderCanvas");
    }

    /// <summary>
    /// カメラ描画をRenderTextureにする設定
    /// </summary>
    protected virtual void cameraLinkRendererTexture()
    {
        Camera.main.targetTexture = renderTexture;
    }

    /// <summary>
    /// カメラ描画を元に戻す設定
    /// </summary>
    protected virtual void cameraUnlinkRendererTexture()
    {
        Camera.main.targetTexture = null;
    }
    

    /// <summary>
    /// 遷移エフェクトの前半
    /// </summary>
    public IEnumerator beforeTransitionCoroutine()
    {
        renderTexture = new RenderTexture( Screen.width, Screen.height, 24 );
        renderTexture.name = "BeforeTransitionRenderTexture";
        transitionCanvas = Instantiate(transitionCanvasPrefab, this.transform);
        transitionCanvas.SetCanvas(renderTexture, transitionMaterial);
        cameraLinkRendererTexture();
        
        Debug.Log("beforeTransitionCoroutine");
        transitionTimeCount = 0;
        transitionEnd = false;
        
        while (transitionTimeCount < transitionTime)
        {
            transitionTimeCount += Time.deltaTime;
            if (transitionTimeCount > transitionTime)
                transitionTimeCount = transitionTime;
            updateMaterial();
            yield return null;
        }

        cameraUnlinkRendererTexture();
    }

    /// <summary>
    /// 遷移エフェクトの後半
    /// </summary>
    public IEnumerator afterTransitionCoroutine()
    {
        renderTexture = new RenderTexture( Screen.width, Screen.height, 24 );
        renderTexture.name = "AfterTransitionRenderTexture";
        transitionCanvas.SetCanvas(renderTexture, transitionMaterial);
        cameraLinkRendererTexture();
        
        Debug.Log("afterTransitionCoroutine");
        transitionTimeCount = transitionTime;
        
        while (transitionTimeCount > 0)
        {
            transitionTimeCount -= Time.deltaTime;
            if (transitionTimeCount < 0)
                transitionTimeCount = 0;
            updateMaterial();
            yield return null;
        }

        // yield return new WaitForSeconds(1.0f);   //描画終了を待つ
        cameraUnlinkRendererTexture();
        Destroy(transitionCanvas.gameObject);
        transitionEnd = true;
    }
    
    private void updateMaterial()
    {
        transitionMaterial.SetFloat("_BlendPercentage", transitionTimeCount/transitionTime);
    }
}

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
    
    private float blendPercentageBefore = 0.0f;
    private float blendPercentageAfter = 1.0f;
    private float blendSpeed = 0.001f;
    private float blendPercentage;
    private bool transitionEnd;
    
    
    public void Initialize(Material setMaterial)
    {
        renderTexture = new RenderTexture( Screen.width, Screen.height, 24 );
        transitionMaterial = setMaterial;

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
        transitionCanvas = Instantiate(transitionCanvasPrefab, this.transform);
        transitionCanvas.SetCanvas(renderTexture, transitionMaterial);
        cameraLinkRendererTexture();
        
        Debug.Log("beforeTransitionCoroutine");
        blendPercentage = 0;
        transitionEnd = false;
        
        while (blendPercentage < blendPercentageAfter)
        {
            blendPercentage += blendSpeed;
            updateMaterial();
            yield return null;
        }

        transitionEnd = true;
    }

    /// <summary>
    /// 遷移エフェクトの後半
    /// </summary>
    public IEnumerator afterTransitionCoroutine()
    {
        cameraLinkRendererTexture();
        Debug.Log("afterTransitionCoroutine");
        while (blendPercentage > blendPercentageBefore)
        {
            blendPercentage -= blendSpeed;
            updateMaterial();
            yield return null;
        }

        Debug.Log("blendPercentage" + blendPercentage);
        yield return new WaitForSeconds(1.0f);   //描画終了を待つ
        // cameraUnlinkRendererTexture();
        Destroy(transitionCanvas.gameObject);
    }
    
    private void updateMaterial()
    {
        Debug.Log("updateMaterial");
        transitionMaterial.SetFloat("_BlendPercentage", blendPercentage);
    }
}

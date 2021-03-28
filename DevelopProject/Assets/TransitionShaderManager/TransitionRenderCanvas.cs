using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionRenderCanvas : MonoBehaviour
{
    [SerializeField] private RawImage renderImage;

    public void SetCanvas(RenderTexture renderTexture, Material transitionMaterial)
    {
        renderImage.texture = renderTexture;
        renderImage.material = transitionMaterial;
    }
}

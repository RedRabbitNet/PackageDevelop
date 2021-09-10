using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEntry : Singleton<TestEntry>
{
    // [SerializeField] private Canvas canvas;
    // [SerializeField] private TestPopup popupPrefab;
    // private TestPopup popup;
    //
    // [SerializeField] private Material transitionMaterial;
    
    IEnumerator Start()
    {
        // popup = Instantiate(popupPrefab, canvas.transform);
        // popup.Initialize();
        // popup.Open();

        yield return null;
        
        // TransitionShaderManager.Instance.Initialize(transitionMaterial);
        // AppSceneManager.Instance.Initialize();

        yield return StartCoroutine(ParticleManager.Instance.InitializeCoroutine());
        ParticleManager.Instance.Play("BlueParticle");
        
        yield return null;
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     popup.Cloase();
        // }
    }
}

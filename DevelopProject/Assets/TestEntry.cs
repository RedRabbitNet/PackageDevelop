﻿using System;
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

        // yield return StartCoroutine(ParticleManager.Instance.InitializeCoroutine());
        // ParticleManager.Instance.Play("BlueParticle");

        yield return StartCoroutine(SoundManager.Instance.InitializeCoroutine());
        SoundManager.Instance.SetVolume(AudioMixerGroupEnum.SE, 1.00f);

        for (int i = 0; i < SoundManager.Instance.ScalePitchCount; i++)
        {
            SoundManager.Instance.PlayWithLoad("piano_c", AudioMixerGroupEnum.SE,0,1.0f, i);
            yield return new WaitForSeconds(2.0f);
        }
        
        
        Debug.Log(SoundManager.Instance.GetVolume(AudioMixerGroupEnum.SE));

        //MonoBehaviour継承クラスをnewしてパラメータだけのコピーが出来るかのテスト
        // AudioSource originSource = new AudioSource();
        // GameObject originObject = new GameObject();
        // originSource.volume = 0.5f;
        //
        // AudioSource copySource = originSource;
        //
        // Debug.Log("originSource.Volume:" + originSource.volume);
        // Debug.Log("copySource.Volume:" + copySource.volume);
        
        
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

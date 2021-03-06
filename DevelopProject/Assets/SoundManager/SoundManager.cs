﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public enum AudioMixerGroupEnum
{
    BGM,
    SE
}

/// <summary>
/// 音を鳴らす仕組みを管理するクラス
/// AudioMixerの利用を前提とする
///
/// (c) 2021 RedRabbit
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    public AudioMixer audioMixer;

    private bool[] isMute;
    private float percentVolume;

    private float minVolume = -20.0f;    //(dB)
    private float maxVolume = 0.0f;      //(dB)

    private Dictionary<string, AudioSource> audioSourceDictionary;    //<filename, AudioSource>

    /// <summary>
    /// 初期化
    /// </summary>
    public IEnumerator InitializeCoroutine()
    {
        audioMixer = Resources.Load<AudioMixer>("AudioMixer");
        audioSourceDictionary = new Dictionary<string, AudioSource>();
        isMute = new bool[Enum.GetNames(typeof(AudioMixerGroupEnum)).Length];

        SceneManager.sceneLoaded += onSceneUnloaded;
        yield return null;
    }

    /// <summary>
    /// シーン切り替え時の処理
    /// </summary>
    private void onSceneUnloaded(Scene beforeScene, LoadSceneMode loadSceneMode)
    {
        /// 再生コルーチンの停止
        /// あくまでもコルーチンの停止であって音源が停止しないことに注意
        /// endActionが呼ばれなくなることに注意
        StopAllCoroutines();
    }
    
    //-------------------------------------------------------------------------------------------------
    //設定

    /// <summary>
    /// Volumeの限界値設定
    /// </summary>
    public void SetLimitedVolume(float setMinVolume = -20.0f, float setMaxVolume = 0.0f)
    {
        minVolume = setMinVolume;
        maxVolume = setMaxVolume;
    }
    
    /// <summary>
    /// 音量割合からdB単位へ変更
    /// </summary>
    private float ConvertVolumeFromPercent(float percentVolume)
    {
        return Mathf.Lerp(minVolume, maxVolume, percentVolume);
    }
    
    /// <summary>
    /// dB単位から音量割合へ変更
    /// </summary>
    private float ConvertPercentFromVolume(float volume)
    {
        return Mathf.InverseLerp(minVolume, maxVolume, volume);
    }

    /// <summary>
    /// 音量の設定
    /// </summary>
    public void SetVolume(AudioMixerGroupEnum audioMixerGroupEnum, float setPercentVolume)
    {
        percentVolume = setPercentVolume;
        audioMixer.SetFloat(audioMixerGroupEnum.ToString(), ConvertVolumeFromPercent(setPercentVolume));
        
        //TODO setPercentVolumeを0で指定した時でも最小値の音量が鳴ってしまうので、ミュート設定を行う
    }

    /// <summary>
    /// 音量の読み込み
    /// </summary>
    public float GetVolume(AudioMixerGroupEnum audioMixerGroupEnum)
    {
        float volume = 0.0f;
        audioMixer.GetFloat(audioMixerGroupEnum.ToString(), out volume);
        return ConvertPercentFromVolume(volume);
    }

    /// <summary>
    /// ミュートの設定
    /// </summary>
    public void SetMute(AudioMixerGroupEnum audioMixerGroupEnum, bool setIsMute)
    {
        isMute[(int) audioMixerGroupEnum] = setIsMute;

        if (isMute[(int) audioMixerGroupEnum])
        {
            audioMixer.SetFloat(audioMixerGroupEnum.ToString(), -80.0f);   
        }
        else
        {
            SetVolume(audioMixerGroupEnum, percentVolume);
        }
    }

    /// <summary>
    /// ミュート状態の取得
    /// </summary>
    public bool GetMute(AudioMixerGroupEnum audioMixerGroupEnum)
    {
        return isMute[(int) audioMixerGroupEnum];
    }
    
    //-------------------------------------------------------------------------------------------------
    //共通処理
    
    
    /// <summary>
    /// 空音源データ作成処理
    /// </summary>
    private AudioSource createAudioSource()
    {
        GameObject audioSourceObject = new GameObject();
        audioSourceObject.name = "audioSourceObject";
        AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        return audioSource;
    }
    
    /// <summary>
    /// 音源データの読み込み処理
    /// </summary>
    private AudioSource loadAudioSource(string directoryPath, string fileName)
    {
        AudioSource audioSource = createAudioSource();
        audioSource.clip = Resources.Load<AudioClip>(directoryPath + fileName);

        return audioSource;
    }

    /// <summary>
    /// 再生処理
    /// </summary>
    private IEnumerator playCoroutine(AudioSource audioSource, float delay, Action endAction = null)
    {
        yield return new WaitForSeconds(delay);

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.time = 0;
        }
        
        audioSource.Play();

        while (audioSource.isPlaying)
        {
            yield return null;   
        }
        endAction?.Invoke();
    }

    
    //-------------------------------------------------------------------------------------------------
    //単体でロードから再生までを処理する

    /// <summary>
    /// 再生処理
    /// </summary>
    public void PlayWithLoad(string fileName, AudioMixerGroupEnum audioMixerGroupEnum, float delay = 0.0f)
    {
        AudioSource audioSource = loadAudioSource("", fileName);
        
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(audioMixerGroupEnum.ToString())[0];

        StartCoroutine(playCoroutine(audioSource, delay, () =>
        {
            Destroy(audioSource.gameObject);
        }));
    }
    //-------------------------------------------------------------------------------------------------
    //一度にロードして保持して置いて、再生する

    /// <summary>
    /// 音源データの読み込み処理
    /// </summary>
    public void LoadAudioSourceToDictionary(string directoryPath, List<string> fileNameList)
    {
        foreach (string fileName in fileNameList)
        {
            AudioSource audioSourceObject = loadAudioSource(directoryPath, fileName);
            DontDestroyOnLoad(audioSourceObject);
            audioSourceDictionary.Add(fileName, audioSourceObject);
        }
    }

    /// <summary>
    /// 再生処理
    /// </summary>
    public void PlayFromDictionary(string fileName, AudioMixerGroupEnum audioMixerGroupEnum, float delay = 0.0f, bool isLoop = false)
    {
        if (!audioSourceDictionary.ContainsKey(fileName))
        {
            Debug.LogWarning("PlayFromDictionary not found filename:" + fileName);
            return;
        }

        audioSourceDictionary[fileName].outputAudioMixerGroup = audioMixer.FindMatchingGroups(audioMixerGroupEnum.ToString())[0];
        audioSourceDictionary[fileName].loop = isLoop;
        
        StartCoroutine(playCoroutine(audioSourceDictionary[fileName], delay, () =>
        {
        }));
    }

    /// <summary>
    /// 停止処理
    /// </summary>
    public void StopFromDictionary(string fileName)
    {
        if (!audioSourceDictionary.ContainsKey(fileName))
        {
            Debug.LogWarning("PlayFromDictionary not found filename:" + fileName);
            return;
        }
        
        audioSourceDictionary[fileName].Stop();
    }

    /// <summary>
    /// 切り替え処理
    /// 再生中の音の停止を再生開始を同時に行う
    /// </summary>
    public void ChangeFromDictionary(string fileName, AudioMixerGroupEnum audioMixerGroupEnum, float delay = 0.0f, bool isLoop = false)
    {
        if (!audioSourceDictionary.ContainsKey(fileName))
        {
            Debug.LogWarning("PlayFromDictionary not found filename:" + fileName);
            return;
        }

        AudioMixerGroup targetAudioMixerGroup = audioMixer.FindMatchingGroups(audioMixerGroupEnum.ToString())[0];
        
        //再生中のすべての音の停止
        Dictionary<string, AudioSource> playingAudioSourceDictionary
            = audioSourceDictionary
                .Where(x => x.Value.outputAudioMixerGroup == targetAudioMixerGroup)
                .Where(x => x.Value.isPlaying == true)
                .ToDictionary(x=>x.Key, x=>x.Value);
        foreach (var audioSourcePair in playingAudioSourceDictionary)
        {
            StopFromDictionary(audioSourcePair.Key);
        }
        
        //再生開始
        audioSourceDictionary[fileName].outputAudioMixerGroup = targetAudioMixerGroup;
        audioSourceDictionary[fileName].loop = isLoop;
        
        StartCoroutine(playCoroutine(audioSourceDictionary[fileName], delay, () =>
        {
        }));
    }
}

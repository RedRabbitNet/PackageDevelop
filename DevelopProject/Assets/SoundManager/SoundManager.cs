using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    private const float minVolume = -80.0f;    //(dB)
    private const float maxVolume = 0.0f;      //(dB)


    /// <summary>
    /// 初期化
    /// </summary>
    public IEnumerator InitializeCoroutine()
    {
        audioMixer = Resources.Load<AudioMixer>("AudioMixer");
        isMute = new bool[Enum.GetNames(typeof(AudioMixerGroupEnum)).Length];
        yield return null;
    }
    
    //-------------------------------------------------------------------------------------------------
    
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
        audioMixer.SetFloat(audioMixerGroupEnum.ToString(), ConvertVolumeFromPercent(setPercentVolume));
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
    }

    /// <summary>
    /// ミュート状態の取得
    /// </summary>
    public bool GetMute(AudioMixerGroupEnum audioMixerGroupEnum)
    {
        return isMute[(int) audioMixerGroupEnum];
    }
    
    
    //-------------------------------------------------------------------------------------------------
    
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
    public void PlayWithLoad(string fileName, AudioMixerGroupEnum audioMixerGroupEnum)
    {
        AudioSource audioSource = loadAudioSource("", fileName);
        
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(audioMixerGroupEnum.ToString())[0];

        StartCoroutine(playCoroutine(audioSource, () =>
        {
            Destroy(audioSource.gameObject);
        }));
    }

    private IEnumerator playCoroutine(AudioSource audioSource, Action endAction = null)
    {
        audioSource.Play();

        while (audioSource.isPlaying)
        {
            yield return null;   
        }
        endAction?.Invoke();
    }

}

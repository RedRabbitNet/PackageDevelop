using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public enum AudioMixerGroupEnum
{
    BGM,
    SE,
    Voice
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

    private float minVolume = -96.0f;    //(dB)
    private float maxVolume = 0.0f;      //(dB)

    private Dictionary<string, AudioSource> audioSourceDictionary;    //<filename, AudioSource>

    private const float defaultAudioSourceVolume = 1.0f;
    private const float defaultAudioSourcePitch = 1.0f;
    
    private List<float> scalePirch = new List<float>()
    {
        1.0f,
        1.125f,
        1.2599f,
        1.3348f,
        1.4983f,
        1.6818f,
        1.8877f,
        2.0f,
    };
    public int ScalePitchCount => scalePirch.Count;

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
        return Mathf.Clamp(Mathf.Log10(Mathf.Clamp(percentVolume, 0f, 1f)) * 20.0f, minVolume, maxVolume);
    }
    
    /// <summary>
    /// dB単位から音量割合へ変更
    /// </summary>
    private float ConvertPercentFromVolume(float volume)
    {
        //0除算回避
        if (volume == 0.0f)
            return 1.0f;
        
        return Mathf.Pow(10,volume / 20.0f);
    }

    /// <summary>
    /// 音量の設定
    /// </summary>
    public void SetVolume(AudioMixerGroupEnum audioMixerGroupEnum, float setPercentVolume)
    {
        percentVolume = setPercentVolume;
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
    /// 音源データの初期設定
    /// </summary>
    private void audioSourceInitialize(AudioSource audioSource)
    {
        audioSource.playOnAwake = false;
        audioSource.time = 0;
        audioSource.volume = defaultAudioSourceVolume;
        audioSource.pitch = defaultAudioSourcePitch;
    }

    /// <summary>
    /// 音源データの設定
    /// </summary>
    private void audioSourceSetParam(AudioSource audioSource, float volume, float pitch)
    {
        audioSource.volume = volume;
        audioSource.pitch = pitch;
    }
    
    /// <summary>
    /// 音源データの読み込み処理
    /// </summary>
    private AudioClip loadAudioClip(string directoryPath, string fileName)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(directoryPath + fileName);
        if (audioClip == null)
        {
            Debug.LogWarning("loadAudioClip missing:" + directoryPath + fileName);   
        }
        
        return audioClip;
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
    public void PlayWithLoad(string fileName, AudioMixerGroupEnum audioMixerGroupEnum, float delay = 0.0f, float volume = defaultAudioSourceVolume, float pitch = defaultAudioSourcePitch)
    {
        AudioSource audioSource = createAudioSource();
        audioSource.clip = loadAudioClip("", fileName);
        
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(audioMixerGroupEnum.ToString())[0];

        audioSourceSetParam(audioSource, volume, pitch);
        

        StartCoroutine(playCoroutine(audioSource, delay, () =>
        {
            //どうせ破棄するので要らない
            // audioSourceInitialize(audioSource);
            Destroy(audioSource.gameObject);
        }));
    }
    public void PlayWithLoadPitchIndex(string fileName, AudioMixerGroupEnum audioMixerGroupEnum, float delay = 0.0f, float volume = defaultAudioSourceVolume, int scalePitchIndex = 0)
    {
        PlayWithLoad(fileName, audioMixerGroupEnum, delay, volume, scalePirch[scalePitchIndex]);
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
            AudioSource audioSource = createAudioSource();
            audioSource.clip = loadAudioClip(directoryPath, fileName);
            DontDestroyOnLoad(audioSource);
            audioSourceDictionary.Add(fileName, audioSource);
        }
    }

    /// <summary>
    /// 再生処理
    /// </summary>
    public void PlayFromDictionary(string fileName, AudioMixerGroupEnum audioMixerGroupEnum, float delay = 0.0f, bool isLoop = false, float volume = defaultAudioSourceVolume, float pitch = defaultAudioSourcePitch)
    {
        if (!audioSourceDictionary.ContainsKey(fileName))
        {
            Debug.LogWarning("PlayFromDictionary not found filename:" + fileName);
            return;
        }

        AudioSource audioSource = audioSourceDictionary[fileName];

        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(audioMixerGroupEnum.ToString())[0];
        audioSource.loop = isLoop;
        
        audioSourceSetParam(audioSource, volume, pitch);
        
        StartCoroutine(playCoroutine(audioSource, delay, () =>
        {
            audioSourceInitialize(audioSource);
        }));
    }
    public void PlayFromDictionaryPitchIndex(string fileName, AudioMixerGroupEnum audioMixerGroupEnum, float delay = 0.0f, bool isLoop = false, float volume = defaultAudioSourceVolume, int scalePitchIndex = 0)
    {
        PlayFromDictionary(fileName, audioMixerGroupEnum, delay, isLoop, volume, scalePirch[scalePitchIndex]);
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ParticleManager : Singleton<ParticleManager>
{
    private Dictionary<string, ParticleSystem> particleResourceDictionary;    //<filename, ParticleSystem>
    private Dictionary<string, ParticleSystem> particleObjectDictionary;    //<filename, ParticleSystem>

    /// <summary>
    /// 初期化
    /// </summary>
    public IEnumerator InitializeCoroutine()
    {
        particleResourceDictionary = new Dictionary<string, ParticleSystem>();
        particleObjectDictionary = new Dictionary<string, ParticleSystem>();

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
        
        //シーン遷移時にオブジェクトは消えるはずなのでクリア
        particleObjectDictionary.Clear();
    }
    
    //-------------------------------------------------------------------------------------------------
    //共通処理

    /// <summary>
    /// データの読み込み処理
    /// </summary>
    private ParticleSystem loadParticleResource(string fileName, bool isPool = false)
    {
        ParticleSystem particleResource;

        //プール指定
        if (isPool)
        {
            if (!particleResourceDictionary.ContainsKey(fileName))
            {
                //リソースプールに存在しないので追加する
                particleResource = Resources.Load<ParticleSystem>(fileName);
                particleResourceDictionary.Add(fileName,particleResource);
            }
            else
            {
                particleResource = particleResourceDictionary[fileName];
            }
        }
        else
        {
            particleResource = Resources.Load<ParticleSystem>(fileName);
        }
        
        return particleResource;
    }
    
    /// <summary>
    /// オブジェクト生成処理
    /// </summary>
    private ParticleSystem instantiateParticleObject(ParticleSystem particleResource, Vector3 position, Quaternion rotation, Transform parent, string fileName ="", bool isPool = false)
    {
        ParticleSystem particleObject;

        //プール指定
        if (isPool)
        {
            if (!particleObjectDictionary.ContainsKey(fileName))
            {
                //オブジェクトプールに存在しないので追加する
                particleObject = Instantiate(particleResource, parent);
                particleObjectDictionary.Add(fileName,particleObject);
            }
            else
            {
                particleObject = particleObjectDictionary[fileName];
            }
        }
        else
        {
            particleObject = Instantiate(particleResource, parent);
        }
        
        particleObject.transform.localPosition = position;
        particleObject.transform.localRotation = rotation;
        
        return particleObject;
    }

    /// <summary>
    /// 再生処理
    /// 再生後オブジェクトは破棄される
    /// </summary>
    private IEnumerator playCoroutine(ParticleSystem particleSystemObject, float delay, Action endAction = null)
    {
        yield return new WaitForSeconds(delay);

        particleSystemObject.Play();    //TODO PlayOnAwakeによる分岐処理をいつか書く

        while (particleSystemObject.isPlaying)
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
    /// <param name="fileName">ファイルパス</param>
    /// <param name="delay">遅延時間</param>
    /// <param name="isResourcePool">リソースのプールフラグ</param>
    /// <param name="isObjectPool">オブジェクトのプールフラグ、falseならDestoryする</param>
    /// <returns>再生ParticleSystem</returns>
    public ParticleSystem Play(string fileName, float delay = 0.0f, bool isResourcePool = false, bool isObjectPool = false)
    {
        return Play(fileName, Vector3.zero, Quaternion.identity, null, delay, isResourcePool, isObjectPool);
    }
    public ParticleSystem Play(string fileName, Vector3 position, Quaternion rotation, Transform parent, float delay = 0.0f, bool isResourcePool = false, bool isObjectPool = false)
    {
        ParticleSystem particleResource = loadParticleResource(fileName, isResourcePool);
        ParticleSystem particleSystemObject = instantiateParticleObject(particleResource, position, rotation, parent, fileName, isObjectPool);

        StartCoroutine(playCoroutine(particleSystemObject, delay, () =>
        {
            if (isObjectPool == false)
            {
                //TODO PartycleSystemのStopActionによるDestroyのほうが望ましいかもしれない
                Destroy(particleSystemObject.gameObject);   
            }
            else
            {
                //Poolするため、生成オブジェクトを破棄せずに停止する
                particleSystemObject.Stop();
                particleSystemObject.time = 0;
            }
        }));

        return particleSystemObject;
    }
    
    //-------------------------------------------------------------------------------------------------
    //一度にロードして保持して置いて、再生する

    /// <summary>
    /// データの読み込み処理
    /// </summary>
    public void LoadParticleResourceToDictionary(string directoryPath, List<string> fileNameList)
    {
        foreach (string fileName in fileNameList)
        { 
            ParticleSystem particleResource = loadParticleResource(directoryPath + fileName, true);
        }
    }
}

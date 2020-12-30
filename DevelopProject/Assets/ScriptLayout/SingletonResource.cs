using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// シングルトンを実現するための抽象クラス
/// シーン遷移後は破棄される
/// Resourcesからクラス名と同じ名前のオブジェクトをロードする
/// </summary>
public abstract class SingletonResource<T> : MonoBehaviour where T : SingletonResource<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            System.Type type = typeof(T);

            TryFindObjectOfType(type);
            TryCreateInstance(type);

            if (instance == null)
            {
                Debug.LogError("Singleton Instance Error");
            }

            DontDestroyOnLoad(instance.gameObject);
            instance.onInitialized();
            return instance;
        }
    }

    private static void TryFindObjectOfType(System.Type type)
    {
        if (instance != null)
            return;

        instance = (T)FindObjectOfType(type);

        if (instance == null)
        {
            Debug.Log("Singleton Find Missing");
        }
        else
        {
            Debug.Log("Singleton Find Success");
        }
    }

    private static void TryCreateInstance(System.Type type)
    {
        if (instance != null)
            return;

        string typeString = type.ToString();
        T component = Resources.Load<T>(typeString);
        instance = Instantiate(component);

        if (instance == null)
        {
            Debug.LogWarning("Singleton Create Error");
        }
        else
        {
            Debug.Log("Singleton Created");
            instance.name = instance.name.Replace("(Clone)", "");
        }
    }

    protected virtual void onInitialized()
    {

    }

    private void OnApplicationQuit()
    {
        Destroy(this.gameObject);
    }
}
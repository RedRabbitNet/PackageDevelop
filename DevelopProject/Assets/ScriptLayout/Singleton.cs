using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// シングルトンを実現するための抽象クラス
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
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
        GameObject obj = new GameObject(typeString, type);
        instance = obj.GetComponent<T>();

        if (instance == null)
        {
            Debug.LogWarning("Singleton Create Error");
        }
        else
        {
            Debug.Log("Singleton Created");
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
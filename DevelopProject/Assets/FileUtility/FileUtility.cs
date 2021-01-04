using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utility;

namespace Utility
{
    /// <summary>
    /// File管理を行うクラス
    /// Jsonから画像データまで広く扱う
    /// </summary>
    public static class FileUtility
    {
        public static Type LoadResourcesJson<Type>(string path)
        {
            string jsonText = Resources.Load<TextAsset>(path).ToString();

            if (String.IsNullOrEmpty(jsonText))
            {
                Debug.LogWarning("LoadJson Error - Path:" + path);
            }
            
            return JsonUtilityExtend.FromJson<Type>(jsonText);
        }
        
        public static Type LoadPersistentJson<Type>(string path) where Type : new()
        {
            path = Application.persistentDataPath + "/" +  path + ".json";
            if ((File.Exists(path) || Directory.Exists(path)))
            {
                string textData = File.ReadAllText(path);

                Type listData = JsonUtilityExtend.FromJson<Type>(textData);
                if (listData == null)
                {
                    Debug.LogWarning("LoadJson Empty - Path:" + path);
                    return new Type();
                }
                else
                {
                    return listData;
                }
            }

            File.WriteAllText(path,"");
            return new Type();
        }
        
        public static void SavePersistentJson<Type>(string path, Type data)
        {
            path = Application.persistentDataPath + "/" +  path + ".json";
            string json = JsonUtilityExtend.ToJson(data);
            Debug.Log("Save:" + json);
            File.WriteAllText(path, json);
        }
        
    }
}
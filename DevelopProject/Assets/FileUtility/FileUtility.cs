using System;
using System.Collections;
using System.Collections.Generic;
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
        public static Type LoadJson<Type>(string path)
        {
            string jsonText = Resources.Load<TextAsset>(path).ToString();

            if (String.IsNullOrEmpty(jsonText))
            {
                Debug.LogWarning("LoadJson Error - Path:" + path);
            }
            
            return JsonUtilityExtend.FromJson<Type>(jsonText);
        }
        
        public static T DeepClone<T>(this T src)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                var binaryFormatter
                    = new System.Runtime.Serialization
                        .Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, src); // シリアライズ
                memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                return (T)binaryFormatter.Deserialize(memoryStream); // デシリアライズ
            }
        }
    }
}
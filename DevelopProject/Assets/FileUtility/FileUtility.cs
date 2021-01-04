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
    }
}
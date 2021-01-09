using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// JsonUtilityの拡張クラス
    /// Listに対応
    /// </summary>
    public static class JsonUtilityExtend
    {
        [System.Serializable]
        private class Wrapper<Type>
        {
            public Type elements;
        }
        private static string wrapStartSymbol = "{\"elements\":";
        private static string wrapEndSymbol = "}";
        
        public static Type FromJson<Type>(string jsonText)
        {
            Type data;
            if (jsonText.StartsWith("["))
            {
                string wrapJsonText = wrapStartSymbol + jsonText + wrapEndSymbol;
                Wrapper<Type> wrapData = JsonUtility.FromJson<Wrapper<Type>>(wrapJsonText);
                data = wrapData.elements;
            }
            else
            {
                data = JsonUtility.FromJson<Type>(jsonText);
            }

            return data;
        }

        public static string ToJson<Type>(Type data)
        {
            string jsonText;

            if (data is IList)
            {
                Wrapper<Type> wrapData = new Wrapper<Type>();
                wrapData.elements = data;
                string wrapJsonText = JsonUtility.ToJson(wrapData);
                
                string temp = wrapJsonText;
                int startIndex = temp.IndexOf(wrapStartSymbol);
                temp = temp.Remove(startIndex, wrapStartSymbol.Length);
                int endIndex = temp.LastIndexOf(wrapEndSymbol);
                temp = temp.Remove(endIndex, wrapEndSymbol.Length);

                jsonText = temp;
            }
            else
            {
                jsonText = JsonUtility.ToJson(data);
            }

            return jsonText;
        }
    }   
}
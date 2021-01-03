using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utility;

/// <summary>
/// ここに定義を記述する
/// ProviderはDataManagerでしか生成しない
/// </summary>
public class MasterDataProvider<DataType> : IMasterDataProvider
{
    // private List<DataType> dataList;
    private string dataPath;

    public MasterDataProvider(string path)//, List<DataType> list)
    {
        // dataList = new List<DataType>();
        // dataList = list;
        dataPath = path;
    }

    public void LoadData()
    {
        // dataList = FileUtility.LoadJson<List<DataType>>(dataPath);    //ここをList内のデータだけコピーするようにしないとリストが上書きされる

        Debug.Log("LoadData:" + typeof(DataType).Name);
        // Debug.Log(JsonUtilityExtend.ToJson(dataList));
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utility;

/// <summary>
/// ここに定義を記述する
/// 機能を追加する場合、IMasterDataProviderにも追加する
/// ProviderはDataManagerでしか生成しない
/// </summary>
public class MasterDataProvider<DataType> : IMasterDataProvider<DataType>
{
    public List<DataType> DataList { get; set; }

    // public List<DataType> dataList;
    private string dataPath;

    public MasterDataProvider(string path)//, List<DataType> list)
    {
        // dataList = new List<DataType>();
        // dataList = list;
        dataPath = path;
    }

    public void LoadData()
    {
        DataList = FileUtility.LoadJson<List<DataType>>(dataPath);
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utility;

/// <summary>
/// ここに定義を記述する
/// 機能を追加する場合、IUserDataProviderにも追加する
/// ProviderはDataManagerでしか生成しない
/// </summary>
public class UserDataProvider<DataType> : IUserDataProvider<DataType>
{
    public List<DataType> DataList { get; set; }
    private string dataPath;

    public UserDataProvider(string path)
    {
        dataPath = path;
    }

    public void LoadData()
    {
        DataList = FileUtility.LoadPersistentJson<List<DataType>>(dataPath);
    }

    public void SaveData()
    {
        FileUtility.SavePersistentJson<List<DataType>>(dataPath, DataList);
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// ここに定義を記述する
/// 機能を追加する場合、IRuntimeDataProviderにも追加する
/// ProviderはDataManagerでしか生成しない
/// </summary>
public class RuntimeDataProvider<DataType> : IRuntimeDataProvider<DataType>
{
    public List<DataType> DataList { get; set; }
    private string dataPath;

    public RuntimeDataProvider()
    {
        DataList = new List<DataType>();
    }
}
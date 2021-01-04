using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ここに実装する機能を宣言する
/// </summary>
public interface IMasterDataProvider
{
    void LoadData();
}
public interface IMasterDataProvider<DataType> : IMasterDataProvider
{
    List<DataType> DataList { get; set; }
}
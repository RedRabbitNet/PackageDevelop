using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ここに実装する機能を宣言する
/// </summary>
public interface IUserDataProvider
{
    void LoadData();
    void SaveData();
}
public interface IUserDataProvider<DataType> : IUserDataProvider
{
    List<DataType> DataList { get; set; }
}
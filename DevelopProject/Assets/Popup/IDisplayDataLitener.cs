using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// データを管理するインターフェース
/// 表示したいデータがある場合に継承する
/// </summary>
public interface IDisplayDataLitener<TData>
{
    TData GetData();
}

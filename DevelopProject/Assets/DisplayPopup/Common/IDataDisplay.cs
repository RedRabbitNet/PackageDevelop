using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// データ受け取り用インターフェース
/// </summary>
public interface IDataListener<TData>
{
    void SetData(TData setData);
}

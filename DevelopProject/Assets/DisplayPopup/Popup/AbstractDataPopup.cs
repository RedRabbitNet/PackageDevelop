using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 継承して使用するポップアップクラス
/// データ保持を兼ねる
/// </summary>
public abstract class AbstractDataPopup<TData> : AbstractPopup, IDataListener<TData>
{
    protected TData data;

    public void SetData(TData setData)
    {
        data = setData;
    }
}

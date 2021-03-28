using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataDisplay<TData>
{
    void SetData(IDisplayDataLitener<TData> dataLitener);
}

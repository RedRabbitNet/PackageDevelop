using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDataDisplay<TData> : MonoBehaviour, IDataDisplay<TData>, IDisplay
{
    protected TData data;
    
    public abstract void initializeView();
    public abstract void updateView();

    public virtual void SetData(IDisplayDataLitener<TData> dataLitener)
    {
        data = dataLitener.GetData();
    }
}

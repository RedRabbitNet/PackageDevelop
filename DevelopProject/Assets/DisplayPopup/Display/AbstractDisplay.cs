using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アプリ内共通表示継承抽象クラス
/// </summary>
public abstract class AbstractDisplay : MonoBehaviour, IDisplay
{
    public abstract void initializeView();
    public abstract void updateView();
}

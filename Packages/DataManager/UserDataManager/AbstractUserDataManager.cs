using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Providerの生成及びアクセスの管理を担う
/// メンバ変数providersを介してすべてのデータへアクセスする
/// foreach等でアクセスすることで、追加データがある際の記述量が減る
///
/// 新しくデータを追加する際は、UserDataProviderを生成し、providersに追加するだけで良い。
/// </summary>
public abstract class AbstractUserDataManager : Singleton<AbstractUserDataManager>
{
    protected Dictionary<string, IUserDataProvider> providers;

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Initialize()
    {
        providers = new Dictionary<string, IUserDataProvider>();
        
        //Provider(データ)の追加
        SetProvider();
    }

    /// <summary>
    /// Providerの追加
    /// </summary>
    protected abstract void SetProvider();

    /// <summary>
    /// 全てのデータをロード
    /// </summary>
    public void AllDataLoad()
    {
        foreach (var data in providers)
        {
            data.Value.LoadData();
        }
    }

    /// <summary>
    /// 全てのデータをセーブ
    /// </summary>
    public void AllDataSave()
    {
        foreach (var data in providers)
        {
            data.Value.SaveData();
        }
    }

    /// <summary>
    /// データ取得
    /// </summary>
    public List<DataType> GetDataList<DataType>()
    {
        return ConvertOriginProvider<UserDataProvider<DataType>>(providers[typeof(DataType).Name]).DataList;
    }

    /// <summary>
    /// Generic派生クラス取得用のコンバータ
    /// </summary>
    protected ProviderType ConvertOriginProvider<ProviderType>(IUserDataProvider provider)
    {
        return (ProviderType)provider;
    }
}

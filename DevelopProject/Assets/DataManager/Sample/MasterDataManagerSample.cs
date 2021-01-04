using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Providerの生成及びアクセスの管理を担う
/// メンバ変数providersを介してすべてのデータへアクセスする
/// foreach等でアクセスすることで、追加データがある際の記述量が減る
///
/// 新しくデータを追加する際は、MasterDataProviderを生成し、providersに追加するだけで良い。
/// </summary>
public class MasterDataManagerSample : AbstractMasterDataManager
{
    /// <summary> 各種データProvider群 </summary>
    private MasterDataProvider<TestData> masterDataProvider;
    private MasterDataProvider<SampleData> sampleDataProvider;
    
    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

        //ロード (初期化後の任意のタイミングで構わない)
        AllDataLoad();
        
        //テストアクセス
        Debug.Log(GetDataList<SampleData>().Find((x)=>x.id == 1).name);
    }

    /// <summary>
    /// Providerの追加
    /// </summary>
    protected override void SetProvider()
    {
        //providerの作成・追加
        masterDataProvider = new MasterDataProvider<TestData>("MasterData/TestData");
        providers.Add(typeof(TestData).Name, masterDataProvider);

        sampleDataProvider = new MasterDataProvider<SampleData>("MasterData/SampleData");
        providers.Add(typeof(SampleData).Name, sampleDataProvider);
    }
}

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
public class MasterDataManager : Singleton<MasterDataManager>
{
    public Dictionary<string, IMasterDataProvider> providers;

    private MasterDataProvider<TestData> masterDataProvider;
    private MasterDataProvider<SampleData> sampleDataProvider;
    
    public  void Initialize()
    {
        providers = new Dictionary<string, IMasterDataProvider>();
        
        //providerの作成・追加
        masterDataProvider = new MasterDataProvider<TestData>("MasterData/TestData");
        providers.Add(typeof(TestData).Name, masterDataProvider);

        sampleDataProvider = new MasterDataProvider<SampleData>("MasterData/SampleData");
        providers.Add(typeof(SampleData).Name, sampleDataProvider);

        //ロード
        foreach (var data in providers)
        {
            data.Value.LoadData();
        }
    }
}

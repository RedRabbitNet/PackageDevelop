using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMasterDataManager : AbstractMasterDataManager
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

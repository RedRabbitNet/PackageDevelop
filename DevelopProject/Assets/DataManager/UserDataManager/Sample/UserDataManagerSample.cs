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
public class UserDataManagerSample : AbstractUserDataManager
{
    /// <summary> 各種データProvider群 </summary>
    private UserDataProvider<UserTestData> testDataProvider;
    private UserDataProvider<UserSampleData> sampleDataProvider;
    
    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

        //ロード (初期状態の構築)
        AllDataLoad();
        
        //リストの追加やデータの追加を行う
        GetDataList<UserSampleData>().Add(new UserSampleData());
        GetDataList<UserSampleData>()[0].id = 1;
        GetDataList<UserSampleData>()[0].name = "SampleDataAdd";
        
        //セーブ
        AllDataSave();
        
        //ロード 書き込み内容の確認
        AllDataLoad();

        
        //テストアクセス
        Debug.Log(GetDataList<UserSampleData>().Find((x)=>x.id == 1).name);
    }

    /// <summary>
    /// Providerの追加
    /// </summary>
    protected override void SetProvider()
    {
        //providerの作成・追加
        testDataProvider = new UserDataProvider<UserTestData>("UserTestData");
        providers.Add(typeof(UserTestData).Name, testDataProvider);

        sampleDataProvider = new UserDataProvider<UserSampleData>("UserSampleData");
        providers.Add(typeof(UserSampleData).Name, sampleDataProvider);
    }
}

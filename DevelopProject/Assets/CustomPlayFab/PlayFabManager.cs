using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PlayFab;
using PlayFab.ClientModels;


public class PlayFabManager : Singleton<PlayFabManager>
{
	private bool isLogin;
	public GetUserInventoryResult inventory;
	public List<PlayerLeaderboardEntry> leaderboardEntryList;

	/// <summary>
	/// ログイン
	/// </summary>
	/// <returns></returns>
	public IEnumerator PlayFabLogIn()
	{
		if (isLogin)
			yield break;
		
		//PlayFabSDKにタイトルが設定されているか確認
		if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
			PlayFabSettings.TitleId = Application.productName;

		//ログインを行う
		var loginFunction = new CustomPlayFabFunction<LoginWithCustomIDRequest, LoginResult>();
		var request = new LoginWithCustomIDRequest { CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true };
		loginFunction.SetRequest = request;
		yield return StartCoroutine(loginFunction.ExecuteCoroutine(PlayFabClientAPI.LoginWithCustomID));
		Debug.Log("PlayFabLogin");
		isLogin = true;
	}

	/// <summary>
	/// ユーザ表示名の更新
	/// </summary>
	public IEnumerator PlayFabUpdateTitleDisplayName(string displayName)
	{
		if (!isLogin)
		{
			Debug.LogWarning("Error:PlayFab not Login");
			yield break;
		}

		var updateTitleDisplayNameFunction = new CustomPlayFabFunction<UpdateUserTitleDisplayNameRequest, UpdateUserTitleDisplayNameResult>();
		var request = new UpdateUserTitleDisplayNameRequest(){ DisplayName = displayName };
		updateTitleDisplayNameFunction.SetRequest = request;
		yield return StartCoroutine(updateTitleDisplayNameFunction.ExecuteCoroutine(PlayFabClientAPI.UpdateUserTitleDisplayName));

		Debug.Log("Result:" + JsonUtility.ToJson(updateTitleDisplayNameFunction.GetResult));
	}

	/// <summary>
	/// インベントリの取得
	/// </summary>
	public IEnumerator PlayFabGetInventory()
	{
		if (!isLogin)
		{
			Debug.LogWarning("Error:PlayFab not Login");
			yield break;
		}

		var getInventoryFunction = new CustomPlayFabFunction<GetUserInventoryRequest, GetUserInventoryResult>();
		var request = new GetUserInventoryRequest();
		getInventoryFunction.SetRequest = request;
		yield return StartCoroutine(getInventoryFunction.ExecuteCoroutine(PlayFabClientAPI.GetUserInventory));

		Debug.Log("Result:" + JsonUtility.ToJson(getInventoryFunction.GetResult));
		inventory = getInventoryFunction.GetResult;
	}

	/// <summary>
	/// カタログアイテムの取得
	/// </summary>
	public IEnumerator PlayFabGetCatalogItem(string catalogVersion)
	{
		var getCatalogFunction = new CustomPlayFabFunction<GetCatalogItemsRequest, GetCatalogItemsResult>();
		var request = new GetCatalogItemsRequest { CatalogVersion = catalogVersion };
		getCatalogFunction.SetRequest = request;
		yield return StartCoroutine(getCatalogFunction.ExecuteCoroutine(PlayFabClientAPI.GetCatalogItems));

		Debug.Log("Result:" + JsonUtility.ToJson(getCatalogFunction.GetResult));
	}

	/// <summary>
	/// ストア情報の取得
	/// </summary>
	public IEnumerator PlayFabGetStore(string storeId, string catalogVersion = "", Action<GetStoreItemsResult> callback = null)
	{
		var getStoreFunction = new CustomPlayFabFunction<GetStoreItemsRequest, GetStoreItemsResult>();
		var request = new GetStoreItemsRequest { StoreId = storeId, CatalogVersion = catalogVersion };
		getStoreFunction.SetRequest = request;
		yield return StartCoroutine(getStoreFunction.ExecuteCoroutine(PlayFabClientAPI.GetStoreItems));

		Debug.Log("Result:" + JsonUtility.ToJson(getStoreFunction.GetResult));
		if (callback != null)
			callback(getStoreFunction.GetResult);
	}
	
	/// <summary>
	/// 統計情報の送信(ランキング登録)
	/// </summary>
	public IEnumerator PlayFabUpdatePlayerStatistics(string stasticName, int score)
	{
		var updateStatisticsFunction = new CustomPlayFabFunction<UpdatePlayerStatisticsRequest, UpdatePlayerStatisticsResult>();
		var request 
			= new UpdatePlayerStatisticsRequest
			{
				Statistics = new List<StatisticUpdate>
				{
					new StatisticUpdate
					{
						StatisticName = stasticName,
						Value = score,	
					},
				}
			};
		updateStatisticsFunction.SetRequest = request;
		yield return StartCoroutine(updateStatisticsFunction.ExecuteCoroutine(PlayFabClientAPI.UpdatePlayerStatistics));

		Debug.Log("Result:" + JsonUtility.ToJson(updateStatisticsFunction.GetResult));
	}
	
	/// <summary>
	/// ランキング取得
	/// </summary>
	public IEnumerator PlayFabGetLeaderboard(string stasticName, int startPosition, int maxResultsCount)
	{
		var getLeaderBoardFunction = new CustomPlayFabFunction<GetLeaderboardRequest, GetLeaderboardResult>();
		var request 
			= new GetLeaderboardRequest
			{
				StatisticName = stasticName,
				StartPosition = startPosition, // 何位以降のランキングを取得するか指定
				MaxResultsCount = maxResultsCount // ランキングデータを何件取得するか指定 最大が100
			};
		getLeaderBoardFunction.SetRequest = request;
		yield return StartCoroutine(getLeaderBoardFunction.ExecuteCoroutine(PlayFabClientAPI.GetLeaderboard));

		Debug.Log("Result:" + JsonUtility.ToJson(getLeaderBoardFunction.GetResult));
		leaderboardEntryList = getLeaderBoardFunction.GetResult.Leaderboard;
	}
	
	/// <summary>
	/// ランキング取得(自身の周囲)
	/// </summary>
	public IEnumerator PlayFabGetLeaderboardAroundPlayer(string stasticName, int maxResultsCount)
	{
		var getLeaderBoardFunction = new CustomPlayFabFunction<GetLeaderboardAroundPlayerRequest, GetLeaderboardAroundPlayerResult>();
		var request 
			= new GetLeaderboardAroundPlayerRequest
			{
				StatisticName = stasticName,
				MaxResultsCount = maxResultsCount // ランキングデータを何件取得するか 自身を含めた数 +-の範囲はMaxResultsCount/2
			};
		getLeaderBoardFunction.SetRequest = request;
		yield return StartCoroutine(getLeaderBoardFunction.ExecuteCoroutine(PlayFabClientAPI.GetLeaderboardAroundPlayer));

		Debug.Log("Result:" + JsonUtility.ToJson(getLeaderBoardFunction.GetResult));
		leaderboardEntryList = getLeaderBoardFunction.GetResult.Leaderboard;
	}
}

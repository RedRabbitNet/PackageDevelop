using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PlayFab;
using PlayFab.SharedModels;
using PlayFab.ClientModels;

public class CustomPlayFabFunction<RequestType,ResultType>
	where RequestType : PlayFabRequestCommon
	where ResultType : PlayFabResultCommon
{
	private RequestType request;
	public RequestType SetRequest { set { request = value; } }
	private ResultType result;
	public ResultType GetResult { get { return result; } }
	private PlayFabError error;
	public PlayFabError GetError { get { return error; } }
	private bool success;
	public bool GetSuccess { get { return success; } }

	private bool isWait;
	
	public IEnumerator ExecuteCoroutine(Action<RequestType, Action<ResultType>, Action<PlayFabError>, object , Dictionary<string, string>> action)
	{
		if (request == null)
			yield break;

		isWait = true;
		action(request, OnSuccess, OnFaild, null, null);
		while (isWait)
		{
			yield return null;
		}
	}
	protected virtual void OnSuccess(ResultType returnResult) { isWait = false; Debug.Log("PlayFabSuccess" + JsonUtility.ToJson(returnResult)); result = returnResult; success = true; }
	protected virtual void OnFaild(PlayFabError returnError) { isWait = false; Debug.Log("PlayFabError:" + returnError.GenerateErrorReport()); error = returnError; success = false; }
}

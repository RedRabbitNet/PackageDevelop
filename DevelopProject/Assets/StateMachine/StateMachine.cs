using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状態遷移を利用するための汎用クラス
/// メンバとして利用する
///
/// copylight 2021 01 25 RedRabbitNet
/// </summary>
public class StateMachine<StateEnum> where StateEnum : System.Enum
{
    private bool isInitialize;
    
    /// <summary> 遷移段階 </summary>
    private enum StateStepEnum : int
    {
        Start,
        Update,
        End
    }
    private StateStepEnum step;

    private Dictionary<StateEnum, Action[]> stepActionList;    //状態ごとのイベント、 遷移段階ごとにActionを設定する
    private StateEnum currentState;
    public StateEnum CurrentState => currentState;
    private StateEnum beforeState;
    public StateEnum BeforeState => beforeState;

    /// <summary>
    /// StateControllerの初期化
    /// </summary>
    public void Initialize()
    {
        if (isInitialize)
        {
            Debug.LogWarning("既にStateControllerは初期化されています");
        }
        
        stepActionList = new Dictionary<StateEnum, Action[]>();
        isInitialize = true;
    }

    /// <summary>
    /// 状態を開始する
    /// 利用開始時に呼ぶ
    /// </summary>
    public void Start(StateEnum targetState)
    {
        ChangeCall(targetState);
    }

    /// <summary>
    /// 状態の変更を行う
    /// </summary>
    public void ChangeCall(StateEnum targetState)
    {
        if (!isInitialize)
        {
            Debug.LogWarning("まだStateControllerの初期化がされていません");
        }

        if (!isContainsState(targetState))
        {
            Debug.LogWarning("該当ステートが設定されていません:" + targetState.ToString());
        }

        beforeState = currentState;
        currentState = targetState;

        step = StateStepEnum.End;
        if (stepActionList[beforeState][(int) StateStepEnum.End] != null)
        {
            stepActionList[beforeState][(int) StateStepEnum.End].Invoke();
        }

        step = StateStepEnum.Start;
        if (stepActionList[currentState][(int) StateStepEnum.Start] != null)
        {
            stepActionList[currentState][(int) StateStepEnum.Start].Invoke();
        }
    }

    /// <summary>
    /// 現在の状態で継続的な呼び出しを行う
    /// </summary>
    public void UpdateCall()
    {
        step = StateStepEnum.Update;
        if (stepActionList[currentState][(int) StateStepEnum.Update] != null)
        {
            stepActionList[currentState][(int) StateStepEnum.Update].Invoke();
        }
    }

    /// <summary>
    /// 状態に対応するイベントを登録する
    /// </summary>
    public void SetState(StateEnum targetState, Action startAction, Action updateAction, Action endAction)
    {
        if (startAction == null && updateAction == null && endAction == null)
        {
            Debug.LogError("イベントなしでステートを設定することはできません");
        }

        if (stepActionList.ContainsKey(targetState))
        {
            Debug.LogError("既にステートが設定されています");
        }

        Action[] stepAction = new Action[Enum.GetNames(typeof(StateStepEnum)).Length];
        if (startAction != null)
        {
            stepAction[(int)StateStepEnum.Start] = startAction;
        }
        if (updateAction != null)
        {
            stepAction[(int)StateStepEnum.Update] = updateAction;
        }
        if (endAction != null)
        {
            stepAction[(int)StateStepEnum.End] = endAction;
        }

        if (stepActionList == null)
        {
            Debug.LogWarning("stepActionList null");
        }
        if (stepAction == null)
        {
            Debug.LogWarning("stepAction null");
        }

        stepActionList.Add(targetState, stepAction);
    }

    /// <summary>
    /// 該当ステートとしてイベントが登録されているか確認する
    /// </summary>
    private bool isContainsState(StateEnum targetState)
    {
        return stepActionList.ContainsKey(targetState);
    }
}

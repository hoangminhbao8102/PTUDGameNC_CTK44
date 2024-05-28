using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission
{
    ///////////////////////////////
    ////////// 멤버 필드 //////////
    ///////////////////////////////

    public enum eState
    {
        NONE, SUCCESS, FAIL, PAUSE
    }

    private List<Action<MissionManager>> _missionList = new List<Action<MissionManager>>();

    private List<string> _missionTextList = new List<string>();

    ///////////////////////////////
    ///////// 멤버 메서드 /////////
    ///////////////////////////////

    public Mission()
    {
        InitMission1();
        InitMission2();
        InitMission3();
    }
    
    private void InitMission1()
    {
        int score = 1000;

        _missionList.Add((missionManager) =>
        {
            if (MissionManager.Instance.State == eState.FAIL)
                return;

            if (missionManager.Score >= score)
                MissionManager.Instance.State = eState.SUCCESS;
        });

        _missionTextList.Add(score.ToString() + "점을 달성하세요!");
    }

    private void InitMission2()
    {
        int score = 4000;

        _missionList.Add((missionManager) =>
        {
            if (MissionManager.Instance.State == eState.FAIL)
                return;

            if (missionManager.Score >= score)
                MissionManager.Instance.State = eState.SUCCESS;
        });

        _missionTextList.Add(score.ToString() + "점을 달성하세요!");
    }

    private void InitMission3()
    {
        int score = 10000;

        _missionList.Add((missionManager) =>
        {
            if (MissionManager.Instance.State == eState.FAIL)
                return;

            if (missionManager.Score >= score)
                MissionManager.Instance.State = eState.SUCCESS;

            //todo : 5층 넘으면 패배
        });

        _missionTextList.Add(score.ToString() + "점을 달성하세요!");
    }

    public void UpdateState(int stage, MissionManager missionManager)
    {
        if(stage < 0)
            Debug.LogError("Out of Range!");

        if (IsUnlimitedMode(stage))
            return;

        _missionList[stage - 1](missionManager);
    }

    public string GetMissionText(int stage)
    {
        return _missionTextList[stage - 1];
    }

    /// <summary>
    /// 무한모드인지 확인
    /// </summary>
    /// <param name="stage"></param>
    /// <returns></returns>
    public bool IsUnlimitedMode(int stage)
    {
        return stage > _missionList.Count;
    }
}


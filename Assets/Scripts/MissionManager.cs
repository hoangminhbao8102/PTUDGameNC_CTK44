using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    private static MissionManager _instance = null;

    public static MissionManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<MissionManager>();

            return _instance;
        }
    }

    /////////////////////////////////
    /////////// 멤버 필드 ///////////
    /////////////////////////////////

    [SerializeField]
    private List<UILabel> _scoreTextList = new List<UILabel>();

    [SerializeField]
    private UILabel _missionText = null;

    [SerializeField]
    private UIPanel _successPanel = null;

    [SerializeField]
    private UIPanel _failPanel = null;

    [SerializeField]
    private PlayerBlock _playerBlock = null;

    [SerializeField]
    private SpeedUp _speedUp = null;

    [SerializeField]
    private float _speedUpTime = 20f; //스피드 증가할 시간

    public int Stage { get; private set; }

    private Mission _mission;

    private int _score;
    public int Score { get { return _score; } }

    private bool _isFinish = false;

    private float _prevTime;

    public Mission.eState State
    {
        get; set;
    }

    ////////////////////////////////
    ////////// 멤버 메서드 /////////
    ////////////////////////////////

    private void Awake()
    {
        State = Mission.eState.NONE;

        _score = 0;

        for (int i = 0; i < _scoreTextList.Count; ++i)
            _scoreTextList[i].text = _score.ToString();

        Stage = Convert.ToInt32(gameObject.scene.name);
        _mission = new Mission();
    }

    private void Start()
    {
        if (_mission.IsUnlimitedMode(Stage)) //무한모드
            _missionText.text = "살아남으세요!";
        else
            _missionText.text = _mission.GetMissionText(Stage);

        _prevTime = Time.time;
    }

    private void Update()
    {
        _mission.UpdateState(Stage, this);

        if (!_isFinish && State == Mission.eState.SUCCESS)
        {
            _isFinish = true;

            //todo : 클리어 시간에 따라 달성도 변경
            PlayerPrefs.SetInt("Stage" + Stage.ToString(), 1);

            //성공 UI 띄우기
            _successPanel.gameObject.SetActive(true);
        }
        
        if(!_isFinish && State == Mission.eState.FAIL)
        {
            _isFinish = true;

            //실패 UI 띄우기
            _failPanel.gameObject.SetActive(true);
        }

        SpeedUpInUnlimitedMode();
    }

    /// <summary>
    /// 매시간마다 속도업
    /// </summary>
    private void SpeedUpInUnlimitedMode()
    {
        //if (_mission.IsUnlimitedMode(Stage) == false)
        //    return;

        //시작 후 경과시간 20초마다 속도 업
        if(Time.time - _prevTime > _speedUpTime)
        {
            _playerBlock.DropSpeedUp();
            _prevTime = Time.time;

            //속도 업 경고창 표시
            _speedUp.Active();
        }
    }

    public void IncreaseScore()
    {
        _score += 1000;

        for (int i = 0; i < _scoreTextList.Count; ++i)
            _scoreTextList[i].text = _score.ToString();
    }
}

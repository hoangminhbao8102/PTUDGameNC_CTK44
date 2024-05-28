using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    [SerializeField]
    private UILabel _numberLabel = null;

    [SerializeField]
    private UIGrid _scoreGrid = null;

    [SerializeField]
    private UITexture _scoreTexture = null;

    [SerializeField]
    private UISprite _lockSprite = null;

    private int _stageNumber;
    private Action<string> _callbackClick = null;
    private bool _isLock = false;

    public void Initialize(ref UIGrid parent, string stageNumber, Action<string> callbackClick)
    {
        _stageNumber = Convert.ToInt32(stageNumber);
        _numberLabel.text = stageNumber;

        _callbackClick = callbackClick;

        //트랜스폼 초기화
        gameObject.transform.parent = parent.transform;
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.localPosition = Vector3.zero;

        GetPlayerPrefs();

        //클릭 콜백 메소드 설정
        UIEventListener.Get(gameObject).onClick = Callback_Click;
    }

    /// <summary>
    /// playerprefs에 저장된 클리어 정보를 가져옴
    /// </summary>
    private void GetPlayerPrefs()
    {
        int score = PlayerPrefs.GetInt("Stage" + _stageNumber.ToString());

        SetGrid(score);
        SetLockState(score);
    }

    /// <summary>
    /// 달성도 표시
    /// </summary>
    /// <param name="score"></param>
    private void SetGrid(int score)
    {
        for(int i =0;i<score;++i)
        {
            var texture = Instantiate(_scoreTexture);

            texture.gameObject.transform.parent = _scoreGrid.gameObject.transform;
            texture.gameObject.transform.localScale = Vector3.one;
            texture.gameObject.transform.localPosition = Vector3.zero;

            texture.gameObject.SetActive(true);
        }

        _scoreGrid.Reposition();
    }

    /// <summary>
    /// 버튼 잠금 여부 세팅
    /// </summary>
    /// <param name="score"></param>
    private void SetLockState(int score)
    {
        _isLock = score > 0 ? false : true;

        //이전 스테이지를 클리어했으면 열림
        int prevStageScore = PlayerPrefs.GetInt("Stage" + (_stageNumber - 1).ToString());
        _isLock = prevStageScore > 0 ? false : true;

        //1스테이지는 항상 열림
        if (_stageNumber == 1)
            _isLock = false;
        
        if (_isLock)
            _lockSprite.gameObject.SetActive(true);
    }

    private void Callback_Click(GameObject stageButton)
    {
        if(!_isLock)
            _callbackClick(_stageNumber.ToString());
    }
}

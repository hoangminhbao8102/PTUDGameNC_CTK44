using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _ui = null;

    void Update()
    {
        //todo : 일시정지 버튼 클릭

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Toggle();
        }       
    }

    /// <summary>
    /// 일시정지로 변경, 일시정지에서 재개
    /// </summary>
    public void Toggle()
    {
        _ui.SetActive(!_ui.activeSelf);

        if(_ui.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private static InputHandler _instance = null;

    public static InputHandler Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<InputHandler>();

            return _instance;
        }
    }

    /////////////////////////////////
    /////////// 멤버 필드 ///////////
    /////////////////////////////////

    public enum DIRECTION
    {
        LEFT, RIGHT, UP, DOWN, COUNT
    }

    [SerializeField]
    private CameraController _cameraController = null; //카메라

    [SerializeField]
    private PlayerBlock _playerBlock = null;

    private Vector3 _touchPosition;
    private float _swipeResistanceX = 100f;
    private float _swipeResistanceY = 100f;
    private DIRECTION _direction = DIRECTION.COUNT;

    private bool _oneClick = false;
    private float _timerForDoubleClick;
    private float _delay = 0.2f;
    private bool _onDoubleClick = false;

    /////////////////////////////////
    /////////// 멤버 메서드 ///////////
    /////////////////////////////////

    private void Update()
    {
        _direction = DIRECTION.COUNT;

        if (Input.GetMouseButtonDown(0))
        {
            _touchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 deltaSwipe = _touchPosition - Input.mousePosition;

            if (Mathf.Abs(deltaSwipe.x) > _swipeResistanceX)
            {
                //x축 스와이프
                _direction = (deltaSwipe.x < 0) ? DIRECTION.RIGHT : DIRECTION.LEFT;
            }

            if (Mathf.Abs(deltaSwipe.y) > _swipeResistanceY)
            {
                //y축 스와이프
                _direction = (deltaSwipe.y < 0) ? DIRECTION.UP : DIRECTION.DOWN;
            }
        }
    }

    /// <summary>
    /// 회전 이벤트시 로테이션 설정
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRotateVector()
    {
        Vector3 rotation = Vector3.zero;

        if (Time.timeScale < Mathf.Epsilon)
            return rotation;

        if (Input.GetKeyDown(KeyCode.Z) || _direction == DIRECTION.UP)
            rotation.x = 90f;
        else if (Input.GetKeyDown(KeyCode.X) || _direction == DIRECTION.RIGHT)
            rotation.y = 90f;
        else if (Input.GetKeyDown(KeyCode.C))
            rotation.z = 90f;
        else if (_direction == DIRECTION.DOWN)
            rotation.x = -90f;
        else if (_direction == DIRECTION.LEFT)
            rotation.y = -90f;

        if(_onDoubleClick)
        {
            rotation.z = 90f;
            _onDoubleClick = false;
        }

        return _cameraController.GetCameraQt() * rotation;
    }

    /// <summary>
    /// 빠르게 내리는 키
    /// </summary>
    public void DropKeyDown()
    {
        _playerBlock.DropPerSecond();
    }

    public void ClickLeftRotButton()
    {
        _cameraController.Rotate(false);
    }

    public void ClickRightRotButton()
    {
        _cameraController.Rotate(true);
    }

    public void ClickRightButton()
    {
        _playerBlock.Move(4, _cameraController.GetCameraQt());
    }

    public void ClickLeftButton()
    {
        _playerBlock.Move(3, _cameraController.GetCameraQt());
    }

    public void ClickUpButton()
    {
        _playerBlock.Move(1, _cameraController.GetCameraQt());
    }

    public void ClickDownButton()
    {
        _playerBlock.Move(2, _cameraController.GetCameraQt());
    }

    public void RotateZAxis()
    {
        if (!_oneClick) // first click no previous clicks
        {
            _oneClick = true;
            _timerForDoubleClick = Time.time; // save the current time
        }
        else
        {
            _oneClick = false; // found a double click, now reset
            _onDoubleClick = true;
        }

        if (_oneClick)
        {
            // if the time now is delay seconds more than when the first click started. 
            if ((Time.time - _timerForDoubleClick) > _delay)
            {
                //basically if thats true its been too long and we want to reset so the next click is simply a single click and not a double click.
                _oneClick = false;
            }
        }
    }
}
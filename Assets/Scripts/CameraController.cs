using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    ///////////////////////////////
    ////////// 멤버 필드 //////////
    ///////////////////////////////

    private Vector3 _center; //카메라 회전 중심점
    private Quaternion _camRotation;
    private float _interpolationAngle;
    private float _angleY;
    private bool _isRotating = false;

    ///////////////////////////////
    ///////// 멤버 메서드 /////////
    ///////////////////////////////

    private void Start()
    {
        //hack : 공전 중심 하드코딩
        _center = new Vector3(3.5f, 3.5f, 3.5f);
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            InputHandler.Instance.ClickRightRotButton();
        //왼쪽 카메라 회전
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            InputHandler.Instance.ClickLeftRotButton();

        //카메라 시점
        transform.forward = Vector3.Normalize(_center - transform.position);

        GetRotation();
    }

    public void Rotate(bool isRight)
    {
        if(!_isRotating)
        {
            _isRotating = true;
            if(isRight)
                StartCoroutine(RevolvesCamera(1));
            else
                StartCoroutine(RevolvesCamera(-1));
        }

        //카메라 시점
        transform.forward = Vector3.Normalize(_center - transform.position);

        GetRotation();
    }
    /// <summary>
    /// 맵 중앙을 중심으로 공전
    /// </summary>
    private IEnumerator RevolvesCamera(int direction)
    {
        float nextAngle = direction * 90f;
        float curAngle = 0f;

        if(direction > 0)
        {
            while(curAngle <= nextAngle)
            {
                float delta = Time.deltaTime * 100.0f * direction;
                curAngle += delta;

                transform.RotateAround(_center, Vector3.down, delta);

                yield return null;
            }
        }
        else
        {
            while(curAngle >= nextAngle)
            {
                float delta = Time.deltaTime * 100.0f * direction;
                curAngle += delta;

                transform.RotateAround(_center, Vector3.down, delta);

                yield return null;
            }
        }

        transform.RotateAround(_center, Vector3.down, -(curAngle - nextAngle));

        _isRotating = false;
    }

    private void GetRotation() //카메라 각도에 따른 컨트롤러 조정 각도
    {
        Vector3 reverseCamForward = new Vector3(transform.forward.x, 0, transform.forward.z);
        _angleY = Quaternion.FromToRotation(Vector3.forward, reverseCamForward).eulerAngles.y;

        if (315 <= _angleY || _angleY < 45) //초기 방향
            _camRotation =  Quaternion.Euler(0, 0, 0);
        else if (45 <= _angleY && _angleY < 135) //오른쪽
            _camRotation = Quaternion.Euler(0, 90.0f, 0);
        else if (135 <= _angleY && _angleY < 225) //뒤쪽
            _camRotation = Quaternion.Euler(0, 180.0f, 0);
        else //왼쪽
            _camRotation = Quaternion.Euler(0, 270.0f, 0);
    }

    public Quaternion GetCameraQt() //조정 각도 반환
    {
        return _camRotation;
    }
}

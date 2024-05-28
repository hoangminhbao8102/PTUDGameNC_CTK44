//#define TEST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//유저가 컨트롤하는 블록
public class PlayerBlock : MonoBehaviour
{
    /////////////////////////////////
    /////////// 멤버 필드 ///////////
    /////////////////////////////////

    [SerializeField]
    private List<GameObject> _blocks = new List<GameObject>(); //블록 리스트

    [SerializeField]
    private float _time = 2.0f; //떨어지는 간격

    [SerializeField]
    private BlockStack _blockStack = null;

    private GameObject _currentBlock = null; //현재 컨트롤하는 블록
    private GameObject _preBlock = null; //미리보기 블록
    private Material _preBlockMaterial = null;

    private bool _isMoveKeyDown = false;

    /////////////////////////////////
    /////////// 멤버 메서드 /////////
    /////////////////////////////////

    private void Start()
    {
        _preBlockMaterial = Resources.Load("PreBlockMaterial", typeof(Material)) as Material;

        Reset();

        InvokeRepeating("DropPerSecond", _time, _time);
    }

    private void Reset()
    {
        //미션 성공 or 실패이면 블록 생성하지 않음
        if (MissionManager.Instance.State != Mission.eState.NONE)
            return;

        //블록 컨테이너에서 하나 선택하여 _currentBlock에 할당
#if !TEST
        int index = Random.Range(0, _blocks.Count);
        _currentBlock = Instantiate(_blocks[index]);
         _preBlock = Instantiate(_blocks[index]);
#else
        _currentBlock = Instantiate(_blocks[4]);
        _preBlock = Instantiate(_blocks[4]);
#endif

        //위치를 가운데 위로 지정
        _currentBlock.transform.position += new Vector3(_blockStack.Length * 0.5f,
                                                        _blockStack.Length,
                                                        _blockStack.Length * 0.5f);

        var preBlockChildren = _preBlock.GetComponentsInChildren<Transform>();
        for(int i =0;i<preBlockChildren.Length;++i)
        {
            preBlockChildren[i].GetComponent<Renderer>().material = _preBlockMaterial;
        }
    }

    /// <summary>
    /// 매초마다 _currentBlock을 내림
    /// </summary>
    public void DropPerSecond()
    {
        if (Time.timeScale < Mathf.Epsilon)
            return;

        if (_currentBlock == null)
            return;

        var curPos = _currentBlock.transform.position;
        curPos.y -= 1.0f;
        _currentBlock.transform.position = curPos;

        //충돌 됐으면 스택에 쌓고 리셋
        if(IsIntersectWhenDropping())
        {
            //큐브 순회하며 Destroy()
            var cubes = _currentBlock.GetComponentsInChildren<Cube>();
            for (int i = 0; i < cubes.Length; ++i)
                cubes[i].Particle();

            _blockStack.StackBlock(_currentBlock);

            Destroy(_currentBlock);
            Destroy(_preBlock);

            Reset();
        }
    }

    /// <summary>
    /// 블록이 밑으로 떨어질 때 교차 체크
    /// </summary>
    /// <returns></returns>
    private bool IsIntersectWhenDropping()
    {
        var blocks = _currentBlock.GetComponentsInChildren<Cube>();

        //hack : LINQ 사용 가능할듯
        for (int i = 0; i < blocks.Length; ++i)
        {
            //바닥에 닿았는지
            if (BlockStack.GetVector3Int(blocks[i].transform.position).y < 0)
                return true;
        }

        //스택과 교차했는지
        if (_blockStack.IsIntersectWith(_currentBlock))
            return true;

        return false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            DropPerSecond();

        //이동, 각도 델타 값
        if (Input.GetKeyDown(KeyCode.W))
            InputHandler.Instance.ClickUpButton();
        else if (Input.GetKeyDown(KeyCode.S))
            InputHandler.Instance.ClickDownButton();
        else if (Input.GetKeyDown(KeyCode.A))
            InputHandler.Instance.ClickLeftButton();
        else if (Input.GetKeyDown(KeyCode.D))
            InputHandler.Instance.ClickRightButton();

        var nextRotation = InputHandler.Instance.GetRotateVector();
        
        if (_currentBlock == null)
            return;

        //hack : 중복된 코드 리팩토링

        //회전
        _currentBlock.transform.Rotate(nextRotation, Space.World);

        bool isCollided = false;

        //스택과 벽에 부딫히는지 검사
        isCollided |= _blockStack.IsIntersectWith(_currentBlock);
        isCollided |= IsOutOfWall(_currentBlock);

        if (isCollided)
        {
            //부딫히면 다시 돌려놓음
            _currentBlock.transform.Rotate(-nextRotation, Space.World);
        }

        SetPreBlockPosition();
    }

    /// <summary>
    /// 키입력에 따라 움직임 (1 상, 2 하, 3 좌, 4 우)
    /// </summary>
    /// <param name="direction"></param>
    public void Move(int direction, Quaternion camQt)
    {
        if (_currentBlock == null)
            return;

        if (_isMoveKeyDown == false)
        {
            _isMoveKeyDown = true;

            Vector3 nextPos = Vector3.zero;

            switch (direction)
            {
                case 1:
                    nextPos.z += 1.0f + Mathf.Epsilon;
                break;
                case 2:
                    nextPos.z -= 1.0f + Mathf.Epsilon;
                break;
                case 3:
                    nextPos.x -= 1.0f + Mathf.Epsilon;
                break;
                case 4:
                    nextPos.x += 1.0f + Mathf.Epsilon;
                break;
            }

            //이동
            var curPos = _currentBlock.transform.position;
            _currentBlock.transform.position = curPos + camQt * nextPos;

            bool isCollided = false;

            //스택과 벽에 부딫히는지 검사
            isCollided |= _blockStack.IsIntersectWith(_currentBlock);
            isCollided |= IsOutOfWall(_currentBlock);

            if (isCollided)
            {
                //부딫히면 다시 돌려놓음
                _currentBlock.transform.position = _currentBlock.transform.position - camQt * nextPos;
            }

            _isMoveKeyDown = false;
        }
    }

    /// <summary>
    /// 블록이 밖으로 나갔는지 검사
    /// </summary>
    /// <returns></returns>
    private bool IsOutOfWall(GameObject block)
    {
        var cubes = block.GetComponentsInChildren<Cube>();

        //hack : LINQ 사용 가능할듯
        bool intersect = false;
        for (int i = 0; i < cubes.Length; ++i)
        {
            //Vector3를 Vector3Int로 변환
            var pos = BlockStack.GetVector3Int(cubes[i].transform.position);
            intersect |= pos.x < 0 || pos.x > _blockStack.Length - 1;
            intersect |= pos.z < 0 || pos.z > _blockStack.Length - 1;
            intersect |= pos.y < 0;
            if (intersect)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 떨어지는 위치를 보여주는 블록의 위치 세팅
    /// </summary>
    private void SetPreBlockPosition()
    {
        int maxPos = (int)(_currentBlock.transform.position.y + 0.5f);

        Vector3 curPos = _currentBlock.transform.position;

        _preBlock.transform.rotation = _currentBlock.transform.rotation;

        //부딫히지 않을때까지 계속 내려가면서 검사
        for (int i =maxPos;i>=0;--i)
        {
            _preBlock.transform.position = new Vector3(curPos.x, i, curPos.z);

            bool isIntersect = false;
            isIntersect |= IsOutOfWall(_preBlock);
            isIntersect |= _blockStack.IsIntersectWith(_preBlock);

            //부딫히면 한칸 위로 올리고 종료
            if (isIntersect)
            {
                int y = Mathf.Min(i + 1, maxPos);
                _preBlock.transform.position = new Vector3(curPos.x, y, curPos.z);
                break;
            }
        }
    }

    /// <summary>
    /// 떨어지는 속도 상승
    /// </summary>
    public void DropSpeedUp()
    {
        if (_time < 0.5f)
            return;

        //hack : 속도 상승률 하드코딩
        CancelInvoke("DropPerSecond");

        _time -= 0.2f;

        InvokeRepeating("DropPerSecond", 0f, _time);
    }
}

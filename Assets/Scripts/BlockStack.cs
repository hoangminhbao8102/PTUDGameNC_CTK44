//#define TEST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//맵에 쌓여진 블록들
public class BlockStack : MonoBehaviour
{
    ///////////////////////////////
    //////// 오토 프로퍼티 ////////
    ///////////////////////////////

    public int Length //스테이지 가로세로 길이
    {
        get; private set;
    }

    ///////////////////////////////
    ////////// 멤버 필드 //////////
    ///////////////////////////////

    [SerializeField]
    private ParticleSystem _particle;

    private Cube[,,] _stack; //블록 스택 컨테이너

    ///////////////////////////////
    ///////// 멤버 메서드 /////////
    ///////////////////////////////

    private void Awake()
    {
#if !TEST
        Length = 8;
#else
        Length = 4;
#endif
        _stack = new Cube[Length, Length, Length];
    }

    private void Start()
    {
        //미리 쌓아놓은 블록을 스택에 추가
        int childCount = transform.childCount;

        //todo : -1씩 y값을 줄여야할듯.
        for(int i =0;i< childCount; ++i)
        {
            var block = transform.GetChild(i);
            StackBlock(block.gameObject);
            Destroy(block.gameObject);
        }
    }

    private void Update()
    {
        while(true)
        {
            //꽉 채워진 층 있는지 확인
            int num = GetFilledLayer();
            if (num == -1)
                break;

            EraseFullFloor(num);

            DownCubes(num);
        }
    }

    /// <summary>
    /// 다 채워진 층이 있다면 층 수 반환
    /// </summary>
    /// <returns></returns>
    private int GetFilledLayer()
    {
        for (int y = 0; y < Length; ++y)
        {
            int count = 0;
            for (int x = 0; x < Length; ++x)
            {
                for (int z = 0; z < Length; ++z)
                {
                    count += _stack[x, y, z] != null ? 1 : 0;
                }
            }
            
            if (count >= Length * Length)
                return y;
        }

        //채워진 층 없다면 -1 리턴
        return -1;
    }

    /// <summary>
    /// 한 층 다 채워지면 지움
    /// </summary>
    /// <param name="layer"></param>
    private void EraseFullFloor(int layer)
    {
        for (int x = 0; x < Length; ++x)
        {
            for (int z = 0; z < Length; ++z)
            {
                //파티클 온
                _stack[x, layer, z].Particle();

                DestroyImmediate(_stack[x, layer, z].gameObject);
            }
        }

        MissionManager.Instance.IncreaseScore();
    }

    /// <summary>
    /// 지워진 칸만큼 밑으로 내림
    /// </summary>
    /// <param name="layer"></param>
    private void DownCubes(int layer)
    {
        for (int y = layer + 1; y < Length; ++y)
        {
            for (int x = 0; x < Length; ++x)
            {
                for (int z = 0; z < Length; ++z)
                {
                    _stack[x, y - 1, z] = _stack[x, y, z];

                    if (_stack[x, y, z] == null)
                        continue;

                    _stack[x, y - 1, z].transform.position = new Vector3(x, y - 1, z);
                }
            }
        }
    }

    /// <summary>
    /// 블록을 쌓음
    /// </summary>
    /// <param name="playerBlock"></param>
    public void StackBlock(GameObject playerBlock)
    {
        //한칸 올림
        var curPos = playerBlock.transform.position;
        playerBlock.transform.position = new Vector3(curPos.x, curPos.y + 1, curPos.z);

        //그 위치에 큐브를 하나씩 배치
        var cubes = playerBlock.GetComponentsInChildren<Cube>();

        //큐브마다 복사하여 맵에 쌓음
        for (int i = 0; i < cubes.Length; ++i)
            StackCube(cubes[i]);
    }

    private void StackCube(Cube cube)
    {
        //position 받아오기, cube만 복사할 경우 월드좌표가 리셋됨
        Vector3 cubePos = cube.transform.position;
        Vector3Int iCubePos = GetVector3Int(cubePos);

        //큐브 객체 복사
        var tempCube = Instantiate(cube);
        tempCube.transform.position = cubePos;

        if (iCubePos.y > Length - 1) //한계치 이상을 쌓을 때 미션 실패!
            MissionManager.Instance.State = Mission.eState.FAIL;
        else //쌓기
            _stack[iCubePos.x, iCubePos.y, iCubePos.z] = tempCube;
    }

    public bool IsIntersectWith(GameObject playerBlock)
    {
        var children = playerBlock.GetComponentsInChildren<Transform>();

        //playerBlock 순회
        for(int i =1;i<children.Length;++i)
        {
            //큐브의 위치에 큐브 스택이 있는지 확인
            Vector3Int cubePos = GetVector3Int(children[i].position);

            bool inMap = true;
            for (int j = 0; j < 3; ++j)
            {
                if (cubePos[j] < 0 || Length - 1 < cubePos[j])
                {
                    inMap = false;
                    break;
                }
            }

            //이동할 위치에 이미 쌓여진 큐브가 존재한다면 교차 판정
            if (inMap && _stack[cubePos.x, cubePos.y, cubePos.z] != null)
                return true;
        }

        return false;
    }

    //hack : ref Vector3 vec이 좋아보이는데 클래스의 멤버 vector3는 ref로 전달이 안된다
    //todo : 위치 리팩토링
    static public Vector3Int GetVector3Int(Vector3 vec)
    {
        Vector3Int iPos = Vector3Int.zero;
        for (int j = 0; j < 3; ++j)
        {
            if(vec[j] < 0)
                iPos[j] = (int)(vec[j] - 0.5f);
            else
                iPos[j] = (int)(vec[j] + 0.5f);
        }

        return iPos;
    }
}


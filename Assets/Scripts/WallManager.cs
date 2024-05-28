using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WallManager : MonoBehaviour
{
    ///////////////////////////////
    ////////// 멤버 필드 //////////
    ///////////////////////////////

    [SerializeField]
    private CameraController _cameraController = null;

    private List<GameObject> _walls = new List<GameObject>();

    ///////////////////////////////
    ///////// 멤버 메서드 /////////
    ///////////////////////////////

    private void Start()
    {
        int wallsCount = transform.childCount;
        for(int i =0;i<wallsCount;++i)
        {
            _walls.Add(transform.GetChild(i).gameObject);
        }
    }
    private void Update()
    {
        //카메라 각도 받아옴
        var angle = _cameraController.GetCameraQt().eulerAngles.y;

        for(int i =0;i<_walls.Count;++i)
            _walls[i].SetActive(true);

        IEnumerable<GameObject> wall = null;

        //각도에 따라 unenable할 벽 결정
        if (Mathf.Abs(angle) <= Mathf.Epsilon)
            wall = _walls.Where(o => o.name == "SouthWall");
        else if (Mathf.Abs(angle - 90) <= Mathf.Epsilon)
            wall = _walls.Where(o => o.name == "WestWall");
        else if (Mathf.Abs(angle - 180) <= Mathf.Epsilon)
            wall = _walls.Where(o => o.name == "NorthWall");
        else if (Mathf.Abs(angle - 270) <= Mathf.Epsilon)
            wall = _walls.Where(o => o.name == "EastWall");

        //리스트 없으면 바로 리턴
        if (wall == null)
            return;

        var wallEnumerator = wall.GetEnumerator();
        while (wallEnumerator.MoveNext())
            wallEnumerator.Current.SetActive(false);
    }
}

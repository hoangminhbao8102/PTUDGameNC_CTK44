using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageScrollList : MonoBehaviour
{
    [SerializeField]
    private UIGrid _grid = null; //스테이지 버튼 그리드

    [SerializeField]
    private string _sceneDirectory = string.Empty; //씬 폴더 경로

    [SerializeField]
    private SceneFader _sceneFader = null;

    [SerializeField]
    private StageButton _buttonPrototype = null;

    private void Start()
    {
        //fixme : 안드로이드에서 씬 파일 불러오는 메소드 찾기

        var stageFiles = Resources.LoadAll("Stages");

        //씬 개수만큼 스테이지 버튼 생성
        int stageCount = stageFiles.Length;

        //hack : 스테이지 개수 하드코딩
        for (int i = 1; i <= 50; ++i)
        {
            string stageNumberString = i.ToString();

            //버튼 생성 및 초기화
            var button = Instantiate(_buttonPrototype);
            button.name = stageNumberString;
            button.Initialize(ref _grid, stageNumberString, Callback_Click);

            if (button != null)
            {
                Debug.Log("Created Button!");
            }
        }

        _grid.Reposition();
    }

    /// <summary>
    /// 확장자 없는 파일명 반환
    /// </summary>
    /// <param name="name"></param>
    private void GetNameWithoutExtension(ref string name)
    {
        int dotIndex = name.LastIndexOf('.');

        name = name.Substring(0, dotIndex);
    }

    /// <summary>
    /// 스크롤 내 버튼 클릭시 콜백 메소드
    /// </summary>
    /// <param name="stage"></param>
    private void Callback_Click(string stage)
    {
        _sceneFader.FadeTo(stage);
    }

    /// <summary>
    /// 플랫폼별 경로에 따른 파일경로 설정
    /// </summary>
    /// <param name="name"></param>
    private string GetNameByPlatform(string name)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(Path.Combine(path, "Documents"), name);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            //string path = "jar:file:/" + Application.dataPath + "!/assets";
            //string path = Application.persistentDataPath;
            string path = Application.streamingAssetsPath;
            return path + name;
        }
        else
        {
            string path = Application.dataPath + "/StreamingAssets";
            return path + name;
        }
    }
}
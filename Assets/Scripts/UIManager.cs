using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private SceneFader _sceneFader = null;

    [SerializeField]
    private UILabel _scoreText = null;

    private string _mainMenuSceneName = "MainMenuScene";

    private void Start()
    {
        SetScoreText();
    }

    private void SetScoreText()
    {
        _scoreText.text = MissionManager.Instance.Score.ToString();
    }

    /// <summary>
    /// 메뉴 버튼 콜백
    /// </summary>
    public void GoToMenu()
    {
        _sceneFader.FadeTo(_mainMenuSceneName);
    }

    /// <summary>
    /// 일시정지 상태에서 메뉴로 갈때
    /// </summary>
    public void GoToMenuInPause()
    {
        Time.timeScale = 1f;
        _sceneFader.FadeTo(_mainMenuSceneName);
    }

    /// <summary>
    /// 다음 버튼 콜백
    /// </summary>
    public void GoToNextStage()
    {
        int next = MissionManager.Instance.Stage + 1;
        _sceneFader.FadeTo(next.ToString());
    }

    /// <summary>
    /// 재시도 콜백
    /// </summary>
    public void Retry()
    {
        _sceneFader.FadeTo(MissionManager.Instance.Stage.ToString());
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string _levelToLoad = "StageScene";

    [SerializeField]
    private string _unlimitedStage = "999";

    [SerializeField]
    private SceneFader _sceneFader = null;

    public void StartStage()
    {
        _sceneFader.FadeTo(_levelToLoad);
    }

    public void StartUnlimited()
    {
        _sceneFader.FadeTo(_unlimitedStage);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

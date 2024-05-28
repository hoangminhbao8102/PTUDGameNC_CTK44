using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonCallbacks : MonoBehaviour
{
    /// <summary>
    /// 메뉴 버튼 콜백
    /// </summary>
    public void GoToMenu()
    {
        Debug.Log("Menu!");
    }

    /// <summary>
    /// 다음 버튼 콜백
    /// </summary>
    public void GoToNextStage()
    {
        Debug.Log("Next Stage!");
    }

    /// <summary>
    /// 재시도 콜백
    /// </summary>
    public void Retry()
    {
        Debug.Log("Retry");
    }

    /// <summary>
    /// 일시정지 재개 콜백
    /// </summary>
    public void Resume()
    {
        Debug.Log("Resume");
    }
}

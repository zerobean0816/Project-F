using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
    public void CallTutorial()
    {
        Debug.Log("[Stage Manager] : Game has started");
        Time.timeScale = 1f;
        SceneManager.LoadScene("TutorialScene");
    }

    public void GameStart()
    {
        Debug.Log("[Stage Manager] : Game has started");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        Debug.Log("[StageManaer] : Quiting Game");
        Application.Quit();
    }
}

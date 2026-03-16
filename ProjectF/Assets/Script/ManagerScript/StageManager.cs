using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    EnemyManager enemyManager;
    UIManager uiManager;
    
    public void ManagerAwake()
    {
        enemyManager = GetComponent<EnemyManager>();
        NullChecker.IsNULL(enemyManager);

        uiManager = GetComponent<UIManager>();
        NullChecker.IsNULL(uiManager);
    }

    public void ManagerUpdate()
    {
        
    }

    public void StopBoss()
    {
        GameObject boss = enemyManager.currentBoss;
        boss.GetComponent<BossMain>().isMoving = false;
    }

    public void MoveBoss()
    {
        GameObject boss = enemyManager.currentBoss;
        boss.GetComponent<BossMain>().isMoving = true;
    }

    public void GameOver()
    {
        Debug.Log("[Stage Manager] : Game is Over");
        Time.timeScale = 0f;
        uiManager.ShowGameOverScreen();
    }

    public void GameStart()
    {
        Debug.Log("[Stage Manager] : Game has started");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    public void CallTutorial()
    {
        Debug.Log("[Stage Manager] : Game has started");
        Time.timeScale = 1f;
        SceneManager.LoadScene("TutorialScene");
    }

    public void GameClear()
    {
        Debug.Log("[Stage Manager] : Player has win the game");
        Time.timeScale = 0f;
        uiManager.ShowGameClearScreen();
    }

    public void GamePause()
    {
        Debug.Log("[Stage Manager] : Player has win the game");
        Time.timeScale = 0f;
        uiManager.ShowPauseSceneByInput();
    }

    public void ExitGame()
    {
        Debug.Log("[StageManaer] : Quiting Game");
        Application.Quit();
    }
}

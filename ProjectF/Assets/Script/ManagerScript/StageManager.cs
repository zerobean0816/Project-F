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
    }

    public void GameClear()
    {
        Debug.Log("[Stage Manager] : Player has win the game");
        Time.timeScale = 0f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
}

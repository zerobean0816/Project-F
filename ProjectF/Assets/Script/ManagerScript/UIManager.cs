using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("PlayerBasicCanvas")]
    [SerializeField] GameObject gameUICanvas;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject pausedScreen;
    [SerializeField] GameObject gameClearScreen;


    public bool isPaused;

    public void ManagerAwake()
    {
        Debug.Log("[UIManager] : UI Manager is Awaked, turing screen off");

        gameUICanvas.SetActive(true);
        gameOverScreen.SetActive(false);
        pausedScreen.SetActive(false);
        gameClearScreen.SetActive(false);
        isPaused = false;
    }

    public void ManagerUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            GameManager.Instance.stageManager.GamePause();
        }
    }

    // public void SetBulletCount(float count)
    // {
       
    // }

    public void ShowGameOverScreen()
    {
        Debug.Log("[UIManager] : Showing Gameover Screen");
        
        gameOverScreen.SetActive(true);
    }

    public void ShowGameClearScreen()
    {
        Debug.Log("[UIManager] : Showing Gameclear Screen");
        
        gameClearScreen.SetActive(true);
    }

    public void ShowPauseSceneByInput()
    {
        pausedScreen.SetActive(isPaused);
    }

    public void CloseAllUI()
    {
        gameUICanvas.SetActive(false);
    }
}

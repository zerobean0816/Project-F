using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("PlayerBasicCanvas")]
    [SerializeField] GameObject playerCanvas;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject pausedScreen;


    public bool isPaused;

    public void ManagerAwake()
    {
        gameOverScreen.SetActive(false);
        pausedScreen.SetActive(false);
        isPaused = false;
    }

    public void ManagerUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }


        ShowPauseSceneByInput();
    }

    // public void SetBulletCount(float count)
    // {
       
    // }

    public void ShowGameOverScreen()
    {
        Debug.Log("[UIManager] : Showing GameOverScreen");
        
        gameOverScreen.SetActive(true);
    }

    public void ShowPauseSceneByInput()
    {      
        pausedScreen.SetActive(isPaused);
    }

}

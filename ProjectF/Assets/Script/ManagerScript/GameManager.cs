using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Sub-Managers")]
    public PlayerManager playerManager { get; private set; }
    public UIManager uIManager{ get; private set; }
    public EnemyManager enemyManager { get; private set; }
    // public SceneManager sceneManager;

    [Header("PublicValueGet")] 
    public Vector2 mousePos {get; private set;}


    private Camera _mainCam;

    void Awake()
    {
        Debug.Log("[GameManager] : Awake is Called");

        SetSingletonForGameManager();
        _mainCam = Camera.main;
        
        ConnectSubManagers();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        SetMousePosForCamera();
        CallSubManager_Update();
    }


    #region On Game Manager Awake
    void SetSingletonForGameManager()
    {
        // Singleton Protection
        if (Instance != null && Instance != this)
        {
            Debug.LogError("[GameManager] : Instance has gameobject that is not this, Destroying gameobject");
            Destroy(gameObject);
            return;
        }

        Debug.Log("[GameManager] : Creating singleton to gameManager");
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region On Sub Manager Awake
    void ConnectSubManagers()
    {
        if (playerManager == null)
        {
            playerManager = GetComponentInChildren<PlayerManager>();
        }

        if (uIManager == null)
        {
            uIManager = GetComponentInChildren<UIManager>();
        }

        if (enemyManager == null)
        {
            enemyManager = GetComponentInChildren<EnemyManager>();
        }
    }

    void CallSubManager_Awake()
    {
        playerManager.ManagerAwake();
    }

    void CallSubManager_Update()
    {
        playerManager.ManagerUpdate();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ManuMenu") return;

        CallSubManager_Awake();

        SetupCamera(scene);
    }

    private void SetupCamera(Scene scene)
    {
        CinemachineCamera vCam = FindFirstObjectByType<CinemachineCamera>();
        if (vCam != null && playerManager.player != null)
        {
            vCam.Follow = playerManager.player.transform;
            vCam.LookAt = playerManager.player.transform;
            Debug.Log("[GameManager] : Camera Linked With player");
        }
        else
        {
            Debug.LogError("[GameManager] : Camera failed to Link to player");
        }
    }

    #endregion

    #region On Update
    void CallSubManagerUpdate()
    {
        playerManager.ManagerUpdate();
    }

    void SetMousePosForCamera()
    {
        if (_mainCam != null)
        {
            mousePos= _mainCam.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            _mainCam = Camera.main;
        }
    }
    #endregion

    #region  GameState
    void GameBossReady()
    {
        
    }

    void GameBossAppear()
    {
        
    }

    #endregion

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;      
    }
}

// public interface ISubManager
// {
//     void ManagerAwake();
//     void ManagerUpdate();
// }
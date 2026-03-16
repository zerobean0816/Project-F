using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] GameObject PlatformGeneratorPerfab;
    [SerializeField] CinemachineCamera cinemachineCameraPrefab;

    [Header("Sub-Managers")]
    public PlayerManager playerManager { get; private set; }
    public UIManager uIManager{ get; private set; }
    public EnemyManager enemyManager { get; private set; }
    public StageManager stageManager {get; private set;}
    // public SceneManager sceneManager;

    [Header("PublicValueGet")] 
    public Vector2 mousePos {get; private set;}
    private Camera _mainCam;

    bool isGameOver = false;
    bool isInGame;

    void Awake()
    {
        Debug.Log("[GameManager] : Awake is Called");

        SetSingletonForGameManager();

        if (Instance == this)
        {
            ConnectSubManagers();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;      
    }


    void Update()
    {
        if (!isInGame) 
        {
            return;   
        }
        if (_mainCam == null)
        {
            Debug.Log("[GameManger] : Camera missing, possibly camera has wrong Tag");
        }

        SetMousePosForCamera();
        CallSubManager_Update();
        CheckPlayerDeath();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _mainCam = Camera.main;
    
        if (scene.name == "MenuScene") 
        {
            Debug.Log("[GameManager] : Set state to Menu");
            isInGame = false;
            uIManager.ReconnectUI();
            uIManager.CloseAllUI();
            return;
        }

        isGameOver = false;
        Debug.Log("[GameManager] : Set state to In Game");

        ConnectSubManagers();
        isInGame = true;

        CallSubManager_Awake();
        SetupCamera(scene);

        if (scene.name != "TutorialScene")
        {
            Instantiate(PlatformGeneratorPerfab);
        }

        Debug.Log("[GameManager] : Loading Platform");
    }


    #region On Game Manager Awake
    void SetSingletonForGameManager()
    {
        // // Singleton Protection
        // if (Instance != null && Instance != this)
        // {
        //     Debug.Log("[GameManager] : Instance has gameobject that is not this, Destroying gameobject");
        //     Destroy(gameObject);
        //     return;
        // }

        // Debug.Log("[GameManager] : Creating singleton to gameManager");
        // Instance = this;
        // DontDestroyOnLoad(gameObject);

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive between scenes
        }
        else
        {
            Destroy(gameObject); // Kill duplicates that spawn when returning to the scene
        }
    }
    #endregion

    #region On Sub Manager Awake
    void ConnectSubManagers()
    {
        if (playerManager == null)
        {
            playerManager = GetComponent<PlayerManager>();
        }

        if (uIManager == null)
        {
            uIManager = GetComponent<UIManager>();
        }

        if (enemyManager == null)
        {
            enemyManager = GetComponent<EnemyManager>();
        }

        if (stageManager == null)
        {
            stageManager = GetComponent<StageManager>();
        }
    }

    void CallSubManager_Awake()
    {
        Debug.Log("[GameManager] : Waking sub- Managers");
        playerManager.ManagerAwake();
        stageManager.ManagerAwake();
        uIManager.ManagerAwake();
    }

    void CallSubManager_Update()
    {
        playerManager.ManagerUpdate();
        stageManager.ManagerUpdate();
        uIManager.ManagerUpdate();
    }

    private void SetupCamera(Scene scene)
    {
        CinemachineCamera vCam = FindFirstObjectByType<CinemachineCamera>();

        if (vCam != null && playerManager.player != null)
        {
            vCam.Follow = playerManager.player.transform;
            vCam.LookAt = playerManager.player.transform;
            vCam.Lens.OrthographicSize = 22f;
            Debug.Log("[GameManager] : Camera Linked With player");
        }
        else
        {
            Debug.Log("[GameManager] : Camera failed to Link to player");
            vCam = Instantiate(cinemachineCameraPrefab);
            vCam.Follow = playerManager.player.transform;
            vCam.LookAt = playerManager.player.transform;
            vCam.Lens.OrthographicSize = 22f;
        }
    }

    #endregion

    #region On Update

    void SetMousePosForCamera()
    {
        if (_mainCam != null)
        {
            mousePos= _mainCam.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log(mousePos);
        }
        else
        {
            Debug.Log("[GameManager] : Failed to load Camera, setting Cmara again");
            _mainCam = Camera.main;
        }
    }
    #endregion

    #region  GameState

    void CheckPlayerDeath()
    {
        if (!playerManager.is_Alive && !isGameOver)
        {
            isGameOver = true;
            stageManager.GameOver();
        }
    }

    #endregion
}

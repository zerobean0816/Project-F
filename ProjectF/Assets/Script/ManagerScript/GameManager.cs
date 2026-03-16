using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] GameObject PlatformGeneratorPerfab;

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
        SetMousePosForCamera();

        if (isInGame)
        {
            CallSubManager_Update();

            CheckPlayerDeath();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _mainCam = Camera.main;

        if (scene.name == "MenuScene") 
        {
            isInGame = false;
            return;
        }

        isInGame = true;

        CallSubManager_Awake();
        SetupCamera(scene);

        Debug.Log("[GameManager] : Loading Platform");
        Instantiate(PlatformGeneratorPerfab);
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
        playerManager.ManagerAwake();
        stageManager.ManagerAwake();
    }

    void CallSubManager_Update()
    {
        playerManager.ManagerUpdate();
        uIManager.ManagerUpdate();
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

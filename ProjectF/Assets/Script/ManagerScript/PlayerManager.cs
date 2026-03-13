using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    public GameObject player {get; private set;}

    // Player State
    public bool _isAlive;
    public bool is_Alive {
        get{ return _isAlive;}

        private set
        {
            _isAlive = value;
            if (_isAlive)
            {
                Debug.Log("[PlayerManager] : Player is Dead!");
            }
        }
    }

    public void ManagerAwake()
    {
        Debug.Log("[PlayerManager] : Awake is Called");
        InitalizePlayer();
    }

    public void ManagerUpdate()
    {
        
    }


    private void InitalizePlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.Log("[PlayerManager] : Player is Not in Scene, Loading new Player...");
            player = Instantiate(playerPrefab, Vector2.zero, Quaternion.identity);
        }

        is_Alive = true;
    }

    public void KillPlayer()
    {
        is_Alive = false;
    }

    void OnDestroy()
    {
        is_Alive = false;
    }
}

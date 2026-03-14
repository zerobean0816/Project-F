using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    public GameObject player {get; private set;}
    public Vector2 playerPos;
    public float stunTime = 2f;
    public ShotGun shotGun {get; private set; }

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
    public bool isStuned {get; private set;}
    
    // HP contorl
    private float _hp;
    public float HP
    {
        get{return _hp; }
        set
        {
            _hp = value;
            if (_hp <= 0)
            {
                is_Alive = false;
            }
        }
    }

    public void ManagerAwake()
    {
        Debug.Log("[PlayerManager] : Awake is Called");
        InitalizePlayer();

        shotGun = player.GetComponentInChildren<ShotGun>();

        isStuned = false;
    }

    public void ManagerUpdate()
    {
        playerPos = player.transform.position;
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

    public void GiveStun()
    {
        Debug.Log("[PlayerManager] : Player get stunned!");
        isStuned = true;
        
        player.GetComponent<PlayerState>().ChangePlayerRed();

        StartCoroutine(StunTimeRoutime(stunTime));
    }

    private IEnumerator StunTimeRoutime(float duration)
    {
        yield return new WaitForSeconds(duration);

        isStuned = false;
        Debug.Log("[PlayerManager] : Stun wore off.");

        player.GetComponent<PlayerState>().ResetPlayerColor();
    }

    void OnDestroy()
    {
        is_Alive = false;
    }
}

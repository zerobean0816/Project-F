using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    public GameObject player {get; private set;}
    public Rigidbody2D playerRb;

    public float stunTime = 2f;

    private const float ULTMAX = 50f;
    public float ultValue{get; private set;}
    public ShotGun shotGun {get; private set; }

    // Player State
    public bool _isAlive;
    public bool is_Alive {
        get{ return _isAlive;}

        private set
        {
            _isAlive = value;
            if (!_isAlive)
            {
                Debug.Log("[PlayerManager] : Player is Dead!");
            }
        }
    }
    public bool isStuned {get; private set;}
    
    // HP contorl

    public void ManagerAwake()
    {
        Debug.Log("[PlayerManager] : Awake is Called");
        InitalizePlayer();

        shotGun = player.GetComponentInChildren<ShotGun>();

        ultValue = 0f;

        isStuned = false;
    }

    public void ManagerUpdate()
    {
    }

    // Give Player Knockback with stun, made for enemy bullet, or enemy it self
    public void GivePlayerKnockBack(Transform otherPos, float stunForce)
    {
        Vector2 direction = ( player.transform.position - otherPos.position ).normalized;

        playerRb.linearVelocity = Vector2.zero;

        playerRb.AddForce(direction * stunForce);
        GameManager.Instance.playerManager.GiveStun();
    }

    private void InitalizePlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
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

    public void AddUltValue(float value)
    {
        ultValue += value;
    }

    void OnDestroy()
    {
        is_Alive = false;
    }
}

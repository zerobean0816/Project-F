using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField]public float maxUltValue = 50f;


    public GameObject player {get; private set;}
    public Rigidbody2D playerRb;

    public float stunTime = 2f;
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
        // No code yet, maybe for status check

        ultValue += Time.deltaTime;
    }

    // Find Player, if not exist, make one and get player info at awake
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


    // call kill 
    public void KillPlayer()
    {
        is_Alive = false;
    }

    // call stun to player
    public void GiveStun()
    {
        Debug.Log("[PlayerManager] : Player get stunned!");
        isStuned = true;
        
        player.GetComponent<PlayerState>().ChangePlayerRed();

        StartCoroutine(StunTimeRoutime(stunTime));
    }

    // Give Player Knockback with stun, made for enemy bullet, or enemy it self
    public void GivePlayerKnockBack(Transform otherPos, float stunForce)
    {
        // get oppsite direciton 
        Vector2 direction = ( player.transform.position - otherPos.position ).normalized;

        // Reset player rigidbody, making player look like paused for second
        playerRb.linearVelocity = (playerRb.linearVelocity * 0.1f );

        // Add opposite directional force to player
        playerRb.AddForce(direction * stunForce);
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
    public void ResetUltValue()
    {
        ultValue = 0f;
    }

    void OnDestroy()
    {
        is_Alive = false;
    }
}

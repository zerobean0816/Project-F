
using UnityEngine;

public class EnemyLook : MonoBehaviour
{
    [Header("Debug Line On")]
    [SerializeField] bool debugLineIsOn = false;

    [Header("Enemy Stats")]
    public float lookRadius = 30f;
    [SerializeField] float rotationSpeed = 40f;


    [Header("EnemyState")]
    public bool hasfoundPlayer{get; private set;}
    public bool startShooting{get; private set;}

    private float sqrDiatanceBWplayer;

    private GameObject player;
    private EnemyGun enemyGun;
    private Vector2 direction;


    void Start()
    {
        player = GameManager.Instance.playerManager.player;
        enemyGun = GetComponentInChildren<EnemyGun>();
        if (enemyGun == null)
        {
            Debug.LogError("[EnemyLook] : EnemyGum Component is missing in gameObject");
        }

        hasfoundPlayer = false;
        startShooting = false;
    }

    public void LookUpdate()
    {
        TrackPlayer();   

        if (sqrDiatanceBWplayer > lookRadius * lookRadius)
        {
            hasfoundPlayer = false;
            return;
        }

        hasfoundPlayer = true;
        RotateGun();
    }

    void TrackPlayer()
    {
        if (player == null)
        {
            Debug.LogError("Player is missing in GameManger");
            return;
        }
        if (enemyGun == null)
        {
            return;
        }

        Vector2 offset = (Vector2)player.transform.position - (Vector2)enemyGun.gun.transform.position ;
        sqrDiatanceBWplayer = offset.SqrMagnitude();
        direction = offset.normalized;

        if (debugLineIsOn)
        {
            DrawTemp(direction);
        }
    }

    void RotateGun()
    {
        Transform gun = enemyGun.gun.transform;

        Vector2 trackDir = (Vector2)player.transform.position - (Vector2)gun.position;
        float angle = Mathf.Atan2(trackDir.y, trackDir.x) * Mathf.Rad2Deg;
        
        Quaternion targetRotaion = Quaternion.Euler(0,0,angle);

        gun.rotation = Quaternion.RotateTowards(
            gun.rotation, targetRotaion, rotationSpeed * Time.deltaTime
        );
    }

    void DrawTemp(Vector2 direction)
    {
        if (sqrDiatanceBWplayer < lookRadius * lookRadius)
        {
            Debug.DrawLine(enemyGun.gun.transform.position, player.transform.position, Color.red);
        }
        else
        {
            Debug.DrawRay(enemyGun.gun.transform.position, direction * lookRadius, Color.green);
        }
    }
}

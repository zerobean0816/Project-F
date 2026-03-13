using UnityEngine;
using UnityEngine.InputSystem;

public class ShotGun : MonoBehaviour
{   
    [SerializeField] float shootStrength = 500f;
    [SerializeField] int maxAmmo = 4;


    public bool isPressed {get; private set;}
    public Vector2 KnockBackForce {get; private set;}

    GameObject player;
    private int currentBullet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.Instance.playerManager.player;
        if (player == null)
        {
            Debug.Log("[ShotGun] : Player is NULL in GmaeManger.PlayerManager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Debug.Log("Shoot!");
        GetKnockBackForce();
    }

    void GetKnockBackForce()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 mousePOs = GameManager.Instance.mousePos;
        Vector2 oppositeDirection = (playerPos- mousePOs).normalized;

        // Debug.Log(" PlayerPosition: " +playerPos);
        // Debug.Log(" MousePosition: " +mousePOs);
        // Debug.Log(" OpDirection:  " +oppositeDirection);

        KnockBackForce = oppositeDirection * KnockBackForce;
    }
}

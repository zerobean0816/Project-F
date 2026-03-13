using UnityEngine;

public class ShotGun : MonoBehaviour
{   
    [SerializeField] float shootStrength = 30;
    [SerializeField] int maxAmmo = 4;


    public bool isPressed;
    public Vector2 KnockBackForce {get; private set;}

    GameObject player;
    private int _currentBullet;
    public int currentBullet
    {
        get
        {
            return _currentBullet;
        }

        private set
        {
            _currentBullet = Mathf.Clamp(value, 0, maxAmmo);
            if (_currentBullet <= 0)
            {
                isShootable = false;
            }
            else
            {
                isShootable = true;
            }
        }
    }
    private bool isShootable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.Instance.playerManager.player;
        if (player == null)
        {
            Debug.Log("[ShotGun] : Player is NULL in GmaeManger.PlayerManager");
        }

        currentBullet = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isShootable)
        {
            isPressed = true;
            Shoot();
            currentBullet --;
        }
        else if (!isShootable)
        {
            ReloadGun();
        }
    }

    void Shoot()
    {
        //Debug.Log("Shotgun Shotted!");
        if (isPressed)
        {
            SetKnockBackForce();
        }
    }
    
    void ReloadGun()
    {
        currentBullet = maxAmmo;
    }

    void SetKnockBackForce()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 mousePOs = GameManager.Instance.mousePos;
        Vector2 oppositeDirection = (playerPos- mousePOs).normalized;

        KnockBackForce = oppositeDirection * shootStrength;

        //Debug.Log($"Opposite Direction {oppositeDirection} | KnockBackFroce: {KnockBackForce}");
    }
}

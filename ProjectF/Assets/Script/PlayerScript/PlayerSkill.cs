using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerSkill : MonoBehaviour, IKnockbackSource
{
    [SerializeField] GameObject powerBullet;

    public float shootStrength = 50f;
    public bool isPressed {get; private set;}

    private GameObject player;
    private Rigidbody2D rb;
    private ShotGun shotgun;

    public bool IsRequesting => isPressed;
    public Vector2 GetForce() => GetKnockBackForce();
    public void Consume() => FinishePress();


    void Start()
    {
        shotgun = gameObject.GetComponentInChildren<ShotGun>();
        player = GameManager.Instance.playerManager.player;
        rb = player.GetComponent<Rigidbody2D>();
    }

    public void CallSkill()
    {
        isPressed = true;
        Instantiate(powerBullet, shotgun.firePoint.position, shotgun.transform.rotation);
    }

    public void FinishePress()
    {
        isPressed = false;
    }

    public Vector2 GetKnockBackForce()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 mousePOs = GameManager.Instance.mousePos;
        Vector2 oppositeDirection = (playerPos- mousePOs).normalized;
        AddRecail(oppositeDirection, out Vector2 finalForce);

        return finalForce * shootStrength;
    }

    private void AddRecail(Vector2 offset , out Vector2 finalForce)
    {
        finalForce = offset;

        if (rb.linearVelocityY < 0 && offset.y > 0.8f)
        {
            finalForce.y += (rb.linearVelocityY * -0.1f);
        }
    }
}

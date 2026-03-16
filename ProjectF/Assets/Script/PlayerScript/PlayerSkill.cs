using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerSkill : MonoBehaviour, IKnockbackSource
{
    [SerializeField] GameObject powerBullet;

    public float shootStrength = 50f;
    public bool isPressed {get; private set;}

    private GameObject player;
    private Rigidbody2D playerRb;
    private ShotGun shotgun;

    public bool IsRequesting => isPressed;
    public Vector2 GetForce() => SetKnockBackForce();
    public void Consume() => FinishePress();


    void Start()
    {
        shotgun = gameObject.GetComponentInChildren<ShotGun>();
        player = GameManager.Instance.playerManager.player;
        playerRb = player.GetComponent<Rigidbody2D>();

        NullChecker.IsNULL(powerBullet);
        NullChecker.IsNULL(shotgun);
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

    Vector2 SetKnockBackForce()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 mousePOs = GameManager.Instance.mousePos;
        Vector2 oppositeDirection = (playerPos- mousePOs).normalized;

        SetRecoilOnYSpeed(oppositeDirection , out Vector2 finalVector);
        Debug.Log($"Opposite Direction {oppositeDirection} | KnockBackFroce: {oppositeDirection * shootStrength}");


        return finalVector * shootStrength;
    }

    void SetRecoilOnYSpeed(Vector2 offset , out Vector2 finalVector)
    {
        finalVector = offset;
        float bonusYForce = 0f;

        if ( offset.y > .1f)
        {
            bonusYForce += GetForceBonusByYSpeed(-10f, -80f, 0.08f );
            bonusYForce += GetFixedByYSpeed(10f, -10f, 1f );
        }
        finalVector.y += bonusYForce;
    }

    float GetForceBonusByYSpeed(float min, float max, float bonusForce)
    {
        float bonus = 0f;

        if ( playerRb.linearVelocityY < min && playerRb.linearVelocityY >= max )
        {
            bonus = Mathf.Abs(playerRb.linearVelocityY) * bonusForce;
        }

        return bonus;
    }

    float GetFixedByYSpeed(float min, float max, float bonusForce)
    {
        float bonus = 0f;

        if ( playerRb.linearVelocityY < min && playerRb.linearVelocityY >= max )
        {
            bonus = bonusForce;
        }

        return bonus;
    }
}

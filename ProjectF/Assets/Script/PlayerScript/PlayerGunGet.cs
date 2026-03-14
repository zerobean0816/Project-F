using UnityEngine;

public class PlayerGunRotator : MonoBehaviour
{
    [SerializeField] GameObject gun;
    Vector2 lookDirection;

    void Start()
    {
    }

    public void GunUpdate()
    {
        if (!GameManager.Instance.playerManager.isStuned)
        {
            GetLookDirection();
            RotateGun();
        }
    }

    void GetLookDirection()
    {
        lookDirection = (GameManager.Instance.mousePos - (Vector2)gun.transform.position).normalized;
    }

    void RotateGun()
    {
        if (gun == null)
        {
            Debug.LogError("[PlaterGunGet] ShotGun is missing here");
        }

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.Euler(0,0,angle);

        Vector3 gunScale = gun.transform.localScale;
        if (angle > 90 || angle < -90)
        {
            gunScale.y = -1f;
        }
        else
        {
            gunScale.y = 1f;
        }
        gun.transform.localScale = gunScale;
    }    
}

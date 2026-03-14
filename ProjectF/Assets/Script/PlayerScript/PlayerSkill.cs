using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField] GameObject powerBullet;

    private ShotGun shotgun;

    void Start()
    {
        shotgun = gameObject.GetComponentInChildren<ShotGun>();
    }

    public void CallSkill()
    {
        Instantiate(powerBullet, shotgun.transform.position, shotgun.transform.rotation);
    }
}

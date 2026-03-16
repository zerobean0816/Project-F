using UnityEngine;

public class BonusReward : MonoBehaviour
{
    [SerializeField] float bonusValue = 30f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.playerManager.AddUltValue(bonusValue);
            Destroy(gameObject);
        }
    }
}

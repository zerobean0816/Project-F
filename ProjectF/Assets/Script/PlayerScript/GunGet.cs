using UnityEngine;

public class GunGet : MonoBehaviour
{
    GameObject player;
    void Start()
    {
        player = GameManager.Instance.playerManager.player;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("[GunGet] : Enable Gun");
            player.GetComponent<PlayerController>().EnableGunControl();

            Destroy(gameObject);
        }
    }
}

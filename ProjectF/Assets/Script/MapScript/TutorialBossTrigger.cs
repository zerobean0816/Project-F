using UnityEngine;

public class TutorialBossTrigger : MonoBehaviour
{
    [SerializeField] GameObject bossPerfab;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("BossSpawned============");
            Vector2 startPos = new Vector2(30f,260f);
            GameObject boss = Instantiate(bossPerfab, startPos, Quaternion.Euler(0,0,-90));
            GameManager.Instance.enemyManager.currentBoss = boss;
            boss.transform.localScale = new Vector3 (15f, 60f, 1f);
        }
    }
}

using UnityEngine;

public class CallGameStartScene : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.stageManager.CallStart();
        }
    }
}

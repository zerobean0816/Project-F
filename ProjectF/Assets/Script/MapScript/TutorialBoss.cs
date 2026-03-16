using Unity.VisualScripting;
using UnityEngine;

public class TutorialBoss : MonoBehaviour
{
    float moveSPeed = 50f;
    Vector2 moveVector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = transform.position;
        moveVector.x += moveSPeed * Time.deltaTime;
        transform.position = moveVector;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}

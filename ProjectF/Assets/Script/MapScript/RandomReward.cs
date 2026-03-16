using UnityEngine;

public class RandomReward : MonoBehaviour
{
    [SerializeField] GameObject enemy_gun;
    [SerializeField] GameObject enemy_saw;
    [SerializeField] GameObject bonusPoint;

    void Start()
    {
        float randomVal = Random.Range(1,4);

        if (randomVal == 1)
        {
            Instantiate(enemy_gun, transform.position, Quaternion.identity);
        }

        if (randomVal == 2)
        {
            Instantiate(enemy_saw, transform.position, Quaternion.identity);
        }

        if (randomVal >=3 )
        {
            Instantiate(bonusPoint, transform.position, Quaternion.identity);
        }
    }
}

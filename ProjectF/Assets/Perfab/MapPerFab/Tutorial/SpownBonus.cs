using System.Collections;
using UnityEngine;

public class SpownBonus : MonoBehaviour
{
    [SerializeField]GameObject bonusTemp;

    [SerializeField]float respawnTime = 2f;


    bool isSpawning;
    GameObject currentSpawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isSpawning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpawn == null && !isSpawning)
        {
            StartCoroutine(SpawnWithDelay(respawnTime));
        }
    }

    IEnumerator SpawnWithDelay(float delay)
    {
        isSpawning = true;
        yield return new WaitForSeconds(delay);

        currentSpawn = Instantiate(bonusTemp, transform.position, Quaternion.identity);

        isSpawning = false;
    }
}

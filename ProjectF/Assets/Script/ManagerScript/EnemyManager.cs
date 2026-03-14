
using UnityEngine;

public enum BossType
{
    Deafult = 0,
    bossType1
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject bossDefault;
    [SerializeField] GameObject bossType1;

    public GameObject currentBoss {get; private set;}
    public Quaternion currentRotate {get; private set;}



    void Start()
    {
        currentRotate =Quaternion.identity;
    }

    public void MakeBoss(Vector2 spawnPoint, bool isUp)
    {
        if (currentBoss != null)
        {
            Debug.Log("[EnemyManager] : Trying to make boss that already exist, ignoring commend");
            return;
        }

        GetDirection(isUp);

        currentBoss = Instantiate(bossDefault, spawnPoint, currentRotate);
    }

    public void ChangeBoss( BossType type ,Vector2 spawnPoint, bool isUp)
    {
        GameObject prefabToSpawn = bossDefault;

        switch (type)
        {
            case BossType.Deafult:
                prefabToSpawn = bossDefault;
                break;
            case BossType.bossType1:
                prefabToSpawn = bossType1;
                break;
        };
        
        GetDirection(isUp);

        Destroy(currentBoss);
        currentBoss = Instantiate(prefabToSpawn, spawnPoint, currentRotate);
    }

    void GetDirection( bool isUp )
    {
        if (!isUp)
        {
            currentRotate = Quaternion.Inverse(currentRotate);
        }
        else
        {
            currentRotate = Quaternion.identity;
        }
    }
}

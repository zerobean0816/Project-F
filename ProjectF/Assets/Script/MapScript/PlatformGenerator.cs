
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlatformGenerator : MonoBehaviour
{
    [Header("Stage forming datas")]
    [SerializeField] List<GameObject> boxCollection;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject floor;

    [Header("")]
    [SerializeField] int firstSpawnCount = 2;
    [SerializeField] int secondSpawnCount = 3;
    [SerializeField] int thirdSpawnCount = 4;


    float listCount;
    float DefaultHeight;
    // float DefaultLength;

    GameObject startStage;
    GameObject player;
    Vector2 spawnPoint;

    void Start()
    {
        startStage = GameObject.Find("StartStage");

        // Get count of boxes that can be spawned
        listCount = boxCollection.Count;

        DefaultHeight = wall.transform.localScale.x;
        // DefaultLength = floor.transform.localScale.x;

        // Get player data from game manager
        player = GameManager.Instance.playerManager.player;

        SetStartSetting();

        MakeWholeStage();
    }

    void SetStartSetting()
    {
        Vector2 startPosition = Vector2.zero;
        if (startStage == null)
        {
            // Set start orgin at foot position of player;
            startPosition = player.transform.position;
            startPosition.y -= 5f;
        }
        else
        {
            startPosition = startStage.transform.position;
        }
        // set spawn point to start position;
        spawnPoint = startPosition;

        Debug.Log("[PlatfromGenerator] : Setting Basic Setting for generation..");
    }

    void MakeWholeStage()
    {
        // Spawn Start Floor
        SpawnStartFloor();

        // Make First stage
        SpawnStageByInput(firstSpawnCount);

        // Make Rest
        SpawnRestStage();

        // Make Second stage
        SpawnStageByInput(secondSpawnCount);

        // Make Rest
        SpawnRestStage();

        // Make Third stage;
        SpawnStageByInput(thirdSpawnCount);

        // Make Final stage / goal
        MakeFinalStage();

        Debug.Log("[PlatfromGenerator] : Stage Spawn Complete!");
    }

    void SpawnStartFloor()
    {
        float floorHeight = floor.transform.localScale.y;

        Vector2 floorOffset = spawnPoint;
        floorOffset.y -= floorHeight / 2;

        Instantiate(floor, floorOffset, Quaternion.identity);
    }

    void SpawnStageByInput(int spawnCount)
    {
        for ( int i = 0; i < spawnCount; i++ )
        {
            MakeRandomBoxOnVector();
            spawnPoint.y += DefaultHeight;
        }
    }

    void MakeRandomBoxOnVector()
    {
        int randomVal = (int)Random.Range(0,listCount);

        Instantiate(boxCollection[randomVal] , spawnPoint, Quaternion.identity);
    }

    void SpawnRestStage()
    {
        // Add Rest Stage
    }

    void MakeFinalStage()
    {
        // Add Final Stage Here
    }
}


using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public PlayerManager Players;
    public UIManager UI;
    public EnemyManager Enemies;

    void Awake() { Instance = this; }

    
}

using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] SpriteRenderer playerBodySprite;

    void Start()
    {
        if (playerBodySprite == null)
        {
            Debug.LogError("[PlayerState] : playerBodySprite is Missing");
        }
    }

    public void ChangePlayerRed()
    {
        //Debug.Log("[PlayerState] : Changing Player to Red..");
        playerBodySprite.color = Color.red;
    }

    public void ResetPlayerColor()
    {
        //Debug.Log("[PlayerState] : Reset Player Color");
        playerBodySprite.color = Color.gray;
    }
}

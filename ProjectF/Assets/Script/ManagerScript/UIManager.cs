
using TMPro;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    public TextMeshPro bulletText;

    void Start()
    {
        if (bulletText == null)
        {
            Debug.LogError("[UIManager] : bulletTextBox is missing!");
        }
    }


    public void SetBulletCount(float count)
    {
        bulletText.text = "Bullet: " + count.ToString();
    }
}

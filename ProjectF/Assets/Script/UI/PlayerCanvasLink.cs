using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasLink : MonoBehaviour
{
    [Header("UI Prefabs")]
    [SerializeField] Slider ultSlider;

    [SerializeField] RawImage rawImage;
    
    float maxUltValue;
    float lastUltValue;

    void Start()
    {
        lastUltValue = -1f;
        maxUltValue = GameManager.Instance.playerManager.maxUltValue;
    }

    void Update()
    {
        float currentUlt = GameManager.Instance.playerManager.ultValue;
        float normalizedultValue = 0f;

        // Only run the heavy logic if the value changed
        if (!Mathf.Approximately(currentUlt, lastUltValue))
        {
            normalizedultValue = Mathf.InverseLerp(0f, maxUltValue, currentUlt);
            lastUltValue = currentUlt;
        }

        ultSlider.value = normalizedultValue;

        rawImage.color = (ultSlider.normalizedValue >= 1.0f) ? Color.green : Color.yellow;
    }
}

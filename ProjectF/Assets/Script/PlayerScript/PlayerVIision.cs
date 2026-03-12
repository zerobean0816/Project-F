using UnityEngine;

public class PlayerVIision : MonoBehaviour
{
    [SerializeField] Transform head;
    void Update()
    {
        LookAtMouse();
    }

    void LookAtMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        Vector2 direction = (Vector2)worldMousePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}

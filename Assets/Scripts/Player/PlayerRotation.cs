using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    private Camera _mainCamera;
    private Rigidbody2D rb;

    void Start()
    {
        _mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        RotateTowardsMouse();
    }

    private void RotateTowardsMouse()
    {
        Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector2)(mouseWorldPos - transform.position);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        rb.MoveRotation(angle);
    }
}


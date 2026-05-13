using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float sneakSpeed = 2f;
    public float walkspeed = 5f;
    public float sprintSpeed = 10f;

    private Rigidbody2D rb;
    private PlayerIntputHandler input;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerIntputHandler>();
    }


    private void FixedUpdate()
    {
        Vector2 direction = input.MoveInput;

        float currentSpeed = walkspeed;

        if (input.IsSprinting)
        {
            currentSpeed = sprintSpeed;
        }
        else if (input.IsSneaking)
        {
            currentSpeed = sneakSpeed;
        }

        rb.MovePosition(rb.position + direction.normalized * currentSpeed * Time.fixedDeltaTime);
    }
}

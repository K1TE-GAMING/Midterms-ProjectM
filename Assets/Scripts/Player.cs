using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    public float sneakSpeed = 2f;
    public float walkispeed = 5f;
    public float sprintSpeed = 10f;
    public PlayerInput playerInput;
    private float currentSpeed;
    private InputAction moveAction;
    private InputAction toggle_sprint;
    private InputAction toggle_sneak;
    private InputAction Fire;
    private InputAction Interact;
    private bool isSprinting = false;
    private bool isSneaking = false;
    private Door currentDoor;

    private Vector2 direction;
    private Vector2 velocity;
    private Rigidbody2D rb;

    [SerializeField] private Transform gunPoint;
    [SerializeField] private GameObject _bulletTrail;
    [SerializeField] private float _weaponRange = 10f;
    [SerializeField] private Animator _muzzleFlashAnimator;



    void Start()
    {
        moveAction = playerInput.actions.FindAction("Move");
        toggle_sprint = playerInput.actions.FindAction("Sprint");
        Fire = playerInput.actions.FindAction("Fire");
        toggle_sneak = playerInput.actions.FindAction("Sneak");
        Interact = playerInput.actions.FindAction("Interact");
        rb = GetComponent<Rigidbody2D>();

       

    }

    void Update()
    {
     
        direction = moveAction.ReadValue<Vector2>();
        isSprinting = toggle_sprint.IsPressed();
        isSneaking = toggle_sneak.IsPressed();

        Shoot();

        RotateTowardsMouse();

        if (currentDoor != null && Interact.WasPressedThisFrame())
        {
            currentDoor.StartOpening();
        }

        if (Interact.WasPressedThisFrame())
        {
            Debug.Log(currentDoor);
        }
    }

    void FixedUpdate()
    {
        currentSpeed = isSprinting ? sprintSpeed : walkispeed;
        currentSpeed = isSneaking ? sneakSpeed : currentSpeed;

        rb.MovePosition(rb.position + direction.normalized * currentSpeed * Time.fixedDeltaTime);
    }

    private void Shoot()
    {
        if ( Input.GetMouseButton(0))
        {
            
            var hit = Physics2D.Raycast(origin: gunPoint.position, direction: gunPoint.right, distance: _weaponRange);

            var bulletTrail = Instantiate(_bulletTrail, gunPoint.position, transform.rotation);

            var trailScript = bulletTrail.GetComponent<BulletTrail>();

            if (hit.collider != null)
            {
                trailScript.SetTargetPosition(hit.point);
                var hittable = hit.collider.GetComponent<IHittable>();
                if (hittable != null)
                {
                    hittable.ReceiveHit(hit);
                }
            }
            else
            {
                var endPosition = gunPoint.position + (gunPoint.right * _weaponRange);
                trailScript.SetTargetPosition(endPosition);
            }

        }
    }


    void RotateTowardsMouse()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        Vector2 aimDirection = (mouseWorldPos - rb.position).normalized;


        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        rb.rotation = angle;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            currentDoor = collision.GetComponent<Door>();
        }
    }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Door"))
            {
                currentDoor = null;
            }
    }
}

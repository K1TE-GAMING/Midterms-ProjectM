using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIntputHandler: MonoBehaviour
{
    public PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction toggle_sprint;
    private InputAction toggle_sneak;
    private InputAction Fire;
    private InputAction Interact;

    public Vector2 MoveInput { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool IsSneaking { get; private set; } 
    public bool IsFiring { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool FirePressedThisFrame { get; internal set; }

    void Awake()
    { 
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        moveAction = playerInput.actions.FindAction("Move");
        toggle_sprint = playerInput.actions.FindAction("Sprint");
        Fire = playerInput.actions.FindAction("Fire");
        toggle_sneak = playerInput.actions.FindAction("Sneak");
        Interact = playerInput.actions.FindAction("Interact");
    }

    void Update()
    {
        MoveInput = moveAction.ReadValue<Vector2>();
        IsSprinting = toggle_sprint.IsPressed();
        IsSneaking = toggle_sneak.IsPressed();
        FirePressedThisFrame = Fire.WasPressedThisFrame();
        InteractPressed = Interact.WasPressedThisFrame();
    }
}



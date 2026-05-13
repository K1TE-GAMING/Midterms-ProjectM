using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Root player component. All behaviour is handled by the dedicated
/// sub-components below — this file is kept for backward compatibility
/// and to hold the PlayerInput reference used by PlayerInputHandler.
///
/// Required components on the same GameObject:
///   - PlayerInputHandler  (reads & exposes all input actions)
///   - PlayerMovement      (Rigidbody2D locomotion)
///   - PlayerRotation      (faces mouse cursor)
///   - PlayerShoot         (raycast weapon)
///   - PlayerInteraction   (door / interactable detection)
/// </summary>
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerIntputHandler))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerRotation))]
[RequireComponent(typeof(PlayerShoot))]
[RequireComponent(typeof(PlayerIntercation))]
public class Player : MonoBehaviour
{
    // No logic lives here anymore.
    // Add any cross-cutting player state (health, stamina, etc.) here as needed.
}

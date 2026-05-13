using UnityEngine;
public class PlayerShoot : MonoBehaviour
{
[Header("Weapon Settings")]

    [SerializeField] private Transform gunPoint;
    [SerializeField] private GameObject bulletTrailPrefab;
    [SerializeField] private float weaponRange = 10f;

    [Header("VFX")]
    [SerializeField] private Animator muzzleFlashAnimator;

    private PlayerIntputHandler input;

    private static readonly int MuzzleFlashTrigger = Animator.StringToHash("MuzzleFlash");

void Start()
{
    input = GetComponent<PlayerIntputHandler>();
}

void Update()
{
    if (input.FirePressedThisFrame)
        Shoot();
     
}

private void Shoot()
{
    // Muzzle flash
    if (muzzleFlashAnimator != null)
        muzzleFlashAnimator.SetTrigger("Shoot");

    // Raycast
    RaycastHit2D hit = Physics2D.Raycast(
        origin: gunPoint.position,
        direction: gunPoint.right,
        distance: weaponRange
    );

    // Spawn trail
    GameObject trailObj = Instantiate(bulletTrailPrefab, gunPoint.position, transform.rotation);
    BulletTrail trailScript = trailObj.GetComponent<BulletTrail>();

    if (hit.collider != null)
    {
        trailScript.SetTargetPosition(hit.point);

        IHittable hittable = hit.collider.GetComponent<IHittable>();
        hittable?.ReceiveHit(hit);
    }
    else
    {
        Vector3 endPosition = gunPoint.position + (gunPoint.right * weaponRange);
        trailScript.SetTargetPosition(endPosition);
    }
}
}
 
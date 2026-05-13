using UnityEngine;

public class EnemyVisibility : MonoBehaviour
{
    [Header("References")]
    public PlayerVisionCone playerVisionCone;   

    [Header("Settings")]
    public bool hideHealthBarWhenInvisible = true;
    public GameObject healthBarObject;          

    private SpriteRenderer[] spriteRenderers;
    private bool isVisible = false;

    void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        if (playerVisionCone == null)
            playerVisionCone = FindObjectOfType<PlayerVisionCone>();

        SetVisible(false);
    }

    void Update()
    {
        bool detected = IsDetectedByPlayer();

        if (detected != isVisible)
            SetVisible(detected);
    }

    bool IsDetectedByPlayer()
    {
        if (playerVisionCone == null) return false;
        return playerVisionCone.IsEnemyVisible(transform);
    }

    void SetVisible(bool visible)
    {
        isVisible = visible;

        foreach (SpriteRenderer sr in spriteRenderers)
            sr.enabled = visible;

        if (healthBarObject != null && hideHealthBarWhenInvisible)
            healthBarObject.SetActive(visible);
    }
}
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PlayerVisionCone : MonoBehaviour
{
    [Header("Cone Shape")]
    public float visionRange = 8f;
    [Range(1f, 179f)]
    public float visionAngle = 90f;
    public int rayCount = 40;         

    [Header("Layer Masks")]
    public LayerMask obstacleLayer;         
    public LayerMask enemyLayer;            

    [Header("References")]
    public Transform gunPoint;              

    [Header("Visuals")]
    public Material coneMaterial;           
    public Color defaultColor = new Color(1f, 1f, 0f, 0.15f);
    public Color detectedColor = new Color(1f, 0f, 0f, 0.25f);

    Mesh coneMesh;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    bool enemyDetected;

    HashSet<Transform> visibleEnemies = new HashSet<Transform>();

   
    public bool IsEnemyVisible(Transform enemy)
    {
        return visibleEnemies.Contains(enemy);
    }

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        coneMesh = new Mesh { name = "VisionCone" };
        meshFilter.mesh = coneMesh;
        meshRenderer.material = coneMaterial;

        if (meshRenderer.material != null)
            meshRenderer.material.color = defaultColor;
    }

    void LateUpdate()
    {
        DrawCone();
        UpdateColor();
    }

    void DrawCone()
    {
        enemyDetected = false;
        visibleEnemies.Clear();

        Vector3 origin = gunPoint.position;
        float halfAngle = visionAngle * 0.5f;
        float angleStep = visionAngle / rayCount;
        float startAngle = GetFacingAngle() - halfAngle;

        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = transform.InverseTransformPoint(origin);

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector2 dir = AngleToDirection(angle);
            Vector3 worldHit = CastRay(origin, dir);

            vertices[i + 1] = transform.InverseTransformPoint(worldHit);

            float dist = Vector2.Distance(origin, worldHit);
            if (dist >= visionRange - 0.05f)          
            {
                RaycastHit2D enemyHit = Physics2D.Raycast(origin, dir, visionRange, enemyLayer);
                if (enemyHit.collider != null)
                    enemyDetected = true;
            }
            else
            {
                RaycastHit2D enemyHit = Physics2D.Raycast(origin, dir, dist, enemyLayer);
                if (enemyHit.collider != null)
                    enemyDetected = true;
            }
        }

        CheckEnemiesInCone(origin);

        for (int i = 0; i < rayCount; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        coneMesh.Clear();
        coneMesh.vertices = vertices;
        coneMesh.triangles = triangles;
        coneMesh.RecalculateNormals();
    }

    Vector3 CastRay(Vector3 origin, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, visionRange, obstacleLayer);
        if (hit.collider != null)
            return hit.point;
        return origin + (Vector3)(direction * visionRange);
    }

    void CheckEnemiesInCone(Vector3 origin)
    {
        Collider2D[] nearby = Physics2D.OverlapCircleAll(origin, visionRange, enemyLayer);
        foreach (Collider2D col in nearby)
        {
            Vector2 toEnemy = (col.transform.position - origin);
            float angle = Vector2.Angle((Vector2)gunPoint.right, toEnemy);
            if (angle <= visionAngle * 0.5f)
            {
                RaycastHit2D los = Physics2D.Raycast(origin, toEnemy.normalized,
                                                     toEnemy.magnitude, obstacleLayer);
                if (los.collider == null)
                {
                    enemyDetected = true;
                    visibleEnemies.Add(col.transform);
                }
            }
        }
    }

    float GetFacingAngle()
    {
        return Mathf.Atan2(gunPoint.right.y, gunPoint.right.x) * Mathf.Rad2Deg;
    }

    Vector2 AngleToDirection(float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    void UpdateColor()
    {
        if (meshRenderer.material == null) return;
        meshRenderer.material.color = enemyDetected ? detectedColor : defaultColor;
    }

    void OnDrawGizmosSelected()
    {
        if (gunPoint == null) return;
        Gizmos.color = Color.yellow;
        float halfAngle = visionAngle * 0.5f;
        float facingAngle = Mathf.Atan2(gunPoint.right.y, gunPoint.right.x) * Mathf.Rad2Deg;
        Vector3 leftEdge = (Vector3)AngleToDirection(facingAngle - halfAngle) * visionRange;
        Vector3 rightEdge = (Vector3)AngleToDirection(facingAngle + halfAngle) * visionRange;
        Gizmos.DrawLine(gunPoint.position, gunPoint.position + leftEdge);
        Gizmos.DrawLine(gunPoint.position, gunPoint.position + rightEdge);
        Gizmos.DrawWireSphere(gunPoint.position, visionRange);
    }
}
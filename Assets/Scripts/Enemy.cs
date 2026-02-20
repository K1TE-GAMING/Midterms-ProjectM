using UnityEngine;

public class Enemy : MonoBehaviour, IHittable
{

    [SerializeField] private int hitpoints;

    public void TakeDamage(RaycastHit2D hitInfo)
    {
        hitpoints -= 1;
        if (hitpoints <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void ReceiveHit(RaycastHit2D hit)
    {
        TakeDamage(hit);
    }


}

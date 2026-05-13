using UnityEngine;

public class Door : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 2f;

    bool isOpening = false;
    bool isfullyOpen = false;

    private float currentAngle = 0f;

    private Collider2D doorCollider;

   public void StartOpening()
    {
        if (!isfullyOpen)
        {
           isOpening = true;
        }

    }

    void Update()
    {
        UpdateDoorCollision();

        if (isOpening && !isfullyOpen)
        {
            currentAngle = Mathf.Lerp(currentAngle, openAngle, Time.deltaTime * openSpeed);

            transform.localRotation = Quaternion.Euler(0, 0, currentAngle);

            if (Mathf.Abs(currentAngle - openAngle) < 1f)
            {
                isfullyOpen = true;
                isOpening = false;
            }
        }

    }

    void UpdateDoorCollision()
    {
        // if door is closed it soild so player and other objects can't pass through it, but when it's open it becomes non-solid so player can pass through it untill it stops opening and becomes solid again
       
        if (currentAngle < 15f)
        {
            if (doorCollider == null)
                doorCollider = GetComponent<Collider2D>();
            if (!doorCollider.enabled)
                doorCollider.enabled = true;
        }
        else
        {
            if (doorCollider != null && doorCollider.enabled)
                doorCollider.enabled = false;
        }
    }
 }



using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 2f;

    bool isOpening = false;
    bool isfullyOpen = false;

    private float currentAngle = 0f;

   public void StartOpening()
    {
        if (!isfullyOpen)
        {
           isOpening = true;
        }

    }

    void Update()
    {
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
}

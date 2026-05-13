using UnityEngine;

public class PlayerIntercation : MonoBehaviour
{
    private PlayerIntputHandler input;
    private Door currentDoor;

    void Start()
    {
        input = GetComponent<PlayerIntputHandler>();
    }

    // Update is called once per frame
    void Update()
    {

        if (currentDoor != null && input.InteractPressed)
        {
            currentDoor.StartOpening();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
           currentDoor = collision.GetComponent<Door>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
            currentDoor = null;
    }
}


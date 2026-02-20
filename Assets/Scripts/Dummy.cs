using UnityEngine;

public class Dummy : MonoBehaviour , IHittable
{
    private Coroutine _activeCoroutine;
    private float _transitionProgress;

    public void ReceiveHit(RaycastHit2D Hit)
    {
        Break();
    }    

    private void Break()
    {
        gameObject.SetActive(false);
    }

}

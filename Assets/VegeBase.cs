using UnityEngine;

public class VegeBase : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) Onhitplayer(collision);
    }

    public virtual void Onhitplayer(Collision collision) { }
}

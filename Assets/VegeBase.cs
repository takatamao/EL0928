using UnityEngine;

public class VegeBase : MonoBehaviour
{
    /// <summary>
    /// 当たったオブジェクト
    /// </summary>
    protected GameObject _targetObject;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) Onhitplayer(collision);
    }

    public virtual void Onhitplayer(Collision collision) { }
}

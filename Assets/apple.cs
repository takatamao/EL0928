using UnityEngine;

public class apple : VegeBase
{
    public override void Onhitplayer(Collision collision)
    {
        Debug.Log("あたった");
        Destroy(this.gameObject);
        _targetObject=collision.gameObject;
        _targetObject?.GetComponent<Player>().GnawObjCount();
        //ポイント加算の処理
    }
}

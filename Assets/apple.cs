using UnityEngine;

public class apple : VegeBase
{
    public override void Onhitplayer(Collision collision)
    {
        Debug.Log("あたった");
        Destroy(this.gameObject);

        //ポイント加算の処理
    }
}

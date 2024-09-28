using UnityEngine;

public class apple : VegeBase
{
    public override void Onhitplayer(Collision collision)
    {
        Debug.Log("‚ ‚½‚Á‚½");
        Destroy(this.gameObject);
        _targetObject=collision.gameObject;
        _targetObject?.GetComponent<Player>().GnawObjCount();
        //ƒ|ƒCƒ“ƒg‰ÁZ‚Ìˆ—
    }
}

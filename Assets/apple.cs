using UnityEngine;

public class apple : VegeBase
{
    public override void Onhitplayer(Collision collision)
    {
        Debug.Log("��������");
        Destroy(this.gameObject);
        _targetObject=collision.gameObject;
        _targetObject?.GetComponent<Player>().GnawObjCount();
        //�|�C���g���Z�̏���
    }
}

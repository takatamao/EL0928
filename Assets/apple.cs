using UnityEngine;

public class apple : VegeBase
{
    public override void Onhitplayer(Collision collision)
    {
        Debug.Log("��������");
        Destroy(this.gameObject);

        //�|�C���g���Z�̏���
    }
}

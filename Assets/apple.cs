using Sound;
using UnityEngine;

public class apple : VegeBase
{
    public override void Onhitplayer(Collision collision)
    {
        Debug.Log("��������");
        Destroy(this.gameObject);
        _targetObject=collision.gameObject;
        _targetObject?.GetComponent<Player>().GnawObjCount();

        // enum�̒l�̐����擾
        int enumCount = System.Enum.GetValues(typeof(SE_EatType)).Length;
        // 0����enumCount-1�͈̔͂ŗ����𐶐�
        int randomIndex = Random.Range(0, enumCount);
        //�|�C���g���Z�̏���
        SCR_SoundManager.instance.PlaySE((SE_EatType)randomIndex);
    }
}

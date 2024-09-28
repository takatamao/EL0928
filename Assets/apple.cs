using Sound;
using UnityEngine;

public class apple : VegeBase
{
    public override void Onhitplayer(Collision collision)
    {
        Debug.Log("あたった");
        Destroy(this.gameObject);
        _targetObject=collision.gameObject;
        _targetObject?.GetComponent<Player>().GnawObjCount();

        // enumの値の数を取得
        int enumCount = System.Enum.GetValues(typeof(SE_EatType)).Length;
        // 0からenumCount-1の範囲で乱数を生成
        int randomIndex = Random.Range(0, enumCount);
        //ポイント加算の処理
        SCR_SoundManager.instance.PlaySE((SE_EatType)randomIndex);
    }
}

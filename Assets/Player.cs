using Sound;
using UniRx;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 変数版Subject かじった数
    private ReactiveProperty<int> _gnawReactiveProperty = new ReactiveProperty<int>(0);

    // 監視される部分を公開
    public IReadOnlyReactiveProperty<int> GnawReactiveProperty => _gnawReactiveProperty;
    [SerializeField, Header("進み速度")]
    private float _speed;

    private void Awake()
    {
        SCR_SoundManager.instance.PlayBGM(BGM_Type.TITLE);
        SCR_SoundManager.instance.SetVolumeBGM(0.4f);
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
          this.transform.position=new Vector3(this.transform.position.x, this.transform.position.y,this.transform.position.z-_speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + _speed);
        }
    }
    /// <summary>
    /// かじったオブジェクトを数える
    /// </summary>
    public void GnawObjCount()
    {
        
        _gnawReactiveProperty.Value += 1;
    }
}

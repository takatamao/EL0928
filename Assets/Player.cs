using UniRx;
using UnityEngine;

public class Player : MonoBehaviour
{
    // �ϐ���Subject ����������
    private ReactiveProperty<int> _gnawReactiveProperty = new ReactiveProperty<int>(0);

    // �Ď�����镔�������J
    public IReadOnlyReactiveProperty<int> GnawReactiveProperty => _gnawReactiveProperty;
    [SerializeField, Header("�i�ݑ��x")]
    private float _speed;
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
    /// ���������I�u�W�F�N�g�𐔂���
    /// </summary>
    public void GnawObjCount()
    {
        Debug.Log("Add");
        _gnawReactiveProperty.Value += 1;
    }
}

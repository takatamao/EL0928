using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// GameManager�N���X
/// </summary>
public class SpawnManager : MonoBehaviour
{
    // �ϐ���Subject �]�����Ă��鐔
    private ReactiveProperty<int> _rollingReactiveProperty = new ReactiveProperty<int>(30);

    // �Ď�����镔�������J
    public IReadOnlyReactiveProperty<int> RollingReactiveProperty => _rollingReactiveProperty;


    [SerializeField, Header("3D�̃I�u�W�F�N�g")] private GameObject appleObject;

    [SerializeField, Header("�A�b�v�������҂�����")] private int _spawnTime;

    [SerializeField, Header("�Q�[���I�[�o�[�܂ł̎���")] private int _gameOverTime;

    [SerializeField, Header("Canvas�Q�[���I�[�o�[��")] private GameObject objectCanvas;
    private bool _isSpawn;
    private async void Start()
    {
        //�����t���O��ς���
        _isSpawn = _rollingReactiveProperty.Value > 0;
        objectCanvas.SetActive(false);
        RollingObjSpawn();
    }

    /// <summary>
    /// �]����I�u�W�F�N�g�𐶐�
    /// </summary>
    public async void RollingObjSpawn()
    {
        while (_isSpawn)
        {
            // 1�b��Ƀ��O���o��
            await UniTask.Delay(TimeSpan.FromSeconds(_spawnTime));
            
          
            GameObject obj= Instantiate(appleObject, this.transform);
            float ztransform = UnityEngine.Random.Range(0, 6);
            obj.transform.position = new Vector3(obj.transform.position.x, this.transform.position.y, this.transform.position.z + ztransform);
            
            _rollingReactiveProperty.Value -= 1;

            _isSpawn = _rollingReactiveProperty.Value > 0 ? true : false;
        }
        await UniTask.Delay(TimeSpan.FromSeconds(_gameOverTime));
        objectCanvas.SetActive(true);
    }
}

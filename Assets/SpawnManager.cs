using UnityEngine;
using UniRx;
using System.Threading.Tasks;
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

    // �ϐ���Subject ����������
    private ReactiveProperty<int> _gnawReactiveProperty = new ReactiveProperty<int>(30);

    // �Ď�����镔�������J
    public IReadOnlyReactiveProperty<int> GnawReactiveProperty => _gnawReactiveProperty;

    [SerializeField, Header("3D�̃I�u�W�F�N�g")] private GameObject appleObject;

    [SerializeField, Header("�A�b�v�������҂�����")] private int _spawnTime;

    private bool _isSpawn;
    private async void Start()
    {
        //�����t���O��ς���
        _isSpawn = _rollingReactiveProperty.Value > 0;
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
            Instantiate(appleObject);
            _rollingReactiveProperty.Value -= 1;

            _isSpawn = _rollingReactiveProperty.Value > 0 ? true : false;
        }
        await UniTask.Delay(TimeSpan.FromSeconds(_spawnTime));
    }
}

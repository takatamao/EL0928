using UnityEngine;
using UniRx;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// GameManagerクラス
/// </summary>
public class SpawnManager : MonoBehaviour
{
    // 変数版Subject 転がってくる数
    private ReactiveProperty<int> _rollingReactiveProperty = new ReactiveProperty<int>(30);

    // 監視される部分を公開
    public IReadOnlyReactiveProperty<int> RollingReactiveProperty => _rollingReactiveProperty;

    // 変数版Subject かじった数
    private ReactiveProperty<int> _gnawReactiveProperty = new ReactiveProperty<int>(30);

    // 監視される部分を公開
    public IReadOnlyReactiveProperty<int> GnawReactiveProperty => _gnawReactiveProperty;

    [SerializeField, Header("3Dのオブジェクト")] private GameObject appleObject;

    [SerializeField, Header("アップル生成待ち時間")] private int _spawnTime;

    private bool _isSpawn;
    private async void Start()
    {
        //生成フラグを変える
        _isSpawn = _rollingReactiveProperty.Value > 0;
        RollingObjSpawn();
    }

    /// <summary>
    /// 転がるオブジェクトを生成
    /// </summary>
    public async void RollingObjSpawn()
    {
        while (_isSpawn)
        {
            // 1秒後にログを出力
            await UniTask.Delay(TimeSpan.FromSeconds(_spawnTime));
            Instantiate(appleObject);
            _rollingReactiveProperty.Value -= 1;

            _isSpawn = _rollingReactiveProperty.Value > 0 ? true : false;
        }
        await UniTask.Delay(TimeSpan.FromSeconds(_spawnTime));
    }
}

using TMPro;
using UnityEngine;
using UniRx;

public class UIGroup : MonoBehaviour
{
    [SerializeField, Header("転がるオブジェクトUI")] private TMP_Text _rollingText;
    [SerializeField, Header("かじったオブジェクトUI")] private TMP_Text _gnawText;

    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private Player _player;
    [SerializeField,Header("UI")] private GameObject _gameObject;
    private void Awake()
    {
        _spawnManager
            ?.RollingReactiveProperty
            ?.Subscribe(x => RollingTextUpdate(x));

        _player
           ?.GnawReactiveProperty
           ?.Subscribe(x => GnawTextUpdate(x));
    }

    /// <summary>
    /// 転がるオブジェクトTextの中身を更新
    /// </summary>
    private void RollingTextUpdate(int num)
    {
        _rollingText.text = num.ToString();

    }

    /// <summary>
    /// かじったオブジェクトTextの中身を更新
    /// </summary>
    private void GnawTextUpdate(int num)
    {
        _gnawText.text = num.ToString();
    }
}

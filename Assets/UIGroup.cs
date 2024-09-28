using TMPro;
using UnityEngine;
using UniRx;

public class UIGroup : MonoBehaviour
{
    [SerializeField, Header("�]����I�u�W�F�N�gUI")] private TMP_Text _rollingText;
    [SerializeField, Header("���������I�u�W�F�N�gUI")] private TMP_Text _gnawText;

    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private Player _player;
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
    /// �]����I�u�W�F�N�gText�̒��g���X�V
    /// </summary>
    private void RollingTextUpdate(int num)
    {
        _rollingText.text = num.ToString();
    }

    /// <summary>
    /// ���������I�u�W�F�N�gText�̒��g���X�V
    /// </summary>
    private void GnawTextUpdate(int num)
    {
        _gnawText.text = num.ToString();
    }
}

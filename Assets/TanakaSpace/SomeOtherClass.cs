using UnityEngine;

public class SomeOtherClass : MonoBehaviour
{
    public CutinAppearance cutinAppearance; // CutinAppearance�R���|�[�l���g�ւ̎Q��

    void Update()
    {
        // �X�y�[�X�L�[�������ꂽ��v���n�u�𐶐�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (cutinAppearance != null)
            {
                cutinAppearance.SpawnPrefab(0); // �w�肵���C���f�b�N�X�̃v���n�u�𐶐�
            }
        }
    }
}

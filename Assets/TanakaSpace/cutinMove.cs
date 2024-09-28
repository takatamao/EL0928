using UnityEngine;
using System.Collections.Generic;

public class debugCutin : MonoBehaviour
{
    public float jumpForce = 30f;       // �W�����v�͂��グ��
    public float gravityScale = 3f;     // �d�̓X�P�[���𒲐����ė������x���グ��
    public float lifetime = 10f;        // �I�u�W�F�N�g��������܂ł̎��ԁi�b�j
    public AudioClip jumpSound;         // ���ʉ���AudioClip
    private AudioSource audioSource;    // AudioSource�R���|�[�l���g���i�[����ϐ�

    void Start()
    {
        // �I�[�f�B�I�\�[�X���擾
        audioSource = GetComponent<AudioSource>();

        // ���ʉ����Đ�
        if (jumpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }

        // �J�n���ɏ��Ń^�C�}�[���Z�b�g���A�폜���Ƀ��X�g�������
        Invoke("DestroyAndRemoveFromList", lifetime);

        // Rigidbody2D�̏d�̓X�P�[����ݒ�
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = gravityScale; // �d�̓X�P�[����ύX
        }

        // 1�x�����W�����v�����s
        Jump();
    }

    // �W�����v�������\�b�h
    void Jump()
    {
        // �W�����v�͂�Y�������ɉ�����
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // �W�����v�͂�Y�������ɐݒ�
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else
        {
            Debug.LogWarning("Rigidbody2D���A�^�b�`����Ă��܂���B");
        }
    }

    // �I�u�W�F�N�g���폜���A���X�g������폜����
    void DestroyAndRemoveFromList()
    {
        

        // �e�I�u�W�F�N�g���擾
        Transform parentTransform = transform.parent;

        // �I�u�W�F�N�g��j��
        Destroy(gameObject); // �q�I�u�W�F�N�g���폜

        // �e�I�u�W�F�N�g���폜
        if (parentTransform != null)
        {
            Destroy(parentTransform.gameObject);
        }
    }

}

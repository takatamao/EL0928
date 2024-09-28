using UnityEngine;
using UnityEngine.UI; // UI���g�p���邽�߂ɒǉ�
using System.Collections; // IEnumerator���g�p���邽�߂ɒǉ�

public class CutinMove2 : MonoBehaviour
{
    public float lifetime = 1f; // �I�u�W�F�N�g��������܂ł̎��ԁi�b�j
    public AudioClip moveSound; // ���ʉ���AudioClip
    private AudioSource audioSource; // AudioSource�R���|�[�l���g���i�[����ϐ�

    public float scaleUpDuration = 0.1f; // �X�P�[����0����1.5�܂ł̎���
    public float scaleHoldDuration = 1f; // �X�P�[�����ێ�����鎞��
    public float scaleDownDuration = 0.3f; // �X�P�[����1.5���猳�̃T�C�Y�܂ł̎���
    public Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f); // �ŏI�I�ȃX�P�[��
    private Vector3 initialScale; // �����X�P�[��
    private float elapsedTime = 0f; // �o�ߎ���
    private bool scalingUp = true; // ���݃X�P�[�����g�咆���ǂ���
    private bool holding = false; // �X�P�[�����ێ����Ă��邩�ǂ�

    public float moveSpeed = 100f; // �I�u�W�F�N�g�̈ړ����x

    // �t���b�V���G�t�F�N�g�p��UI�C���[�W
    public Image flashImage; // UI�C���[�W���i�[����ϐ�
    private bool isFlashing = false; // �t���b�V�������ǂ���

    void Start()
    {
        // �I�[�f�B�I�\�[�X���擾
        audioSource = GetComponent<AudioSource>();

        // ���ʉ����Đ�
        if (moveSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(moveSound);
        }

        // �����X�P�[����ۑ�
        initialScale = transform.localScale;

        // �J�n���ɏ��Ń^�C�}�[���Z�b�g���A�폜���Ƀ��X�g�������
        Invoke("DestroyAndRemoveFromList", lifetime);
    }

    void Update()
    {
        // �X�P�[���̕⊮����
        if (scalingUp)
        {
            if (elapsedTime < scaleUpDuration)
            {
                elapsedTime += Time.deltaTime; // �o�ߎ��Ԃ𑝉�������
                float t = elapsedTime / scaleUpDuration; // ���`�⊮�̐i�s�x
                transform.localScale = Vector3.Lerp(initialScale, targetScale, t); // �X�P�[����⊮
            }
            else
            {
                // �X�P�[���̊g�傪����������A�ێ����ԂɑJ��
                scalingUp = false;
                holding = true; // �X�P�[�����ێ������Ԃ�
                elapsedTime = 0f; // �o�ߎ��Ԃ����Z�b�g

                // �t���b�V�����J�n
                StartCoroutine(FlashScreen());
            }
        }
        else if (holding)
        {
            if (elapsedTime < scaleHoldDuration)
            {
                elapsedTime += Time.deltaTime; // �ێ����Ԃ̌o�߂𑝉�������
            }
            else
            {
                // �ێ����Ԃ��o�߂�����A�X�P�[���_�E�����J�n
                holding = false;
                elapsedTime = 0f; // �o�ߎ��Ԃ����Z�b�g
            }
        }
        else
        {
            if (elapsedTime < scaleDownDuration)
            {
                elapsedTime += Time.deltaTime; // �o�ߎ��Ԃ𑝉�������
                float t = elapsedTime / scaleDownDuration; // ���`�⊮�̐i�s�x
                transform.localScale = Vector3.Lerp(targetScale, initialScale, t); // �X�P�[����⊮

                // �X�P�[����0�ȉ��ɂȂ�����폜
                if (transform.localScale.x <= 0f || transform.localScale.y <= 0f || transform.localScale.z <= 0f)
                {
                    DestroyAndRemoveFromList();
                }
            }
            else
            {
                // �X�P�[���_�E�������������ꍇ�͍폜
                DestroyAndRemoveFromList();
            }
        }
    }

    private IEnumerator FlashScreen()
    {
        if (flashImage != null)
        {
            flashImage.gameObject.SetActive(true); // �t���b�V���C���[�W��\��
            flashImage.color = new Color(1f, 1f, 1f, 1f); // ���F�ŕs�����ɐݒ�
            float flashDuration = 0.01f; // �t���b�V���̎�������
            float fadeDuration = 0.1f; // �t���b�V�����瓧���ɖ߂鎞��
            float elapsedFlashTime = 0f;

            // �t���b�V���̃t�F�[�h�A�E�g
            while (elapsedFlashTime < flashDuration)
            {
                elapsedFlashTime += Time.deltaTime;
                yield return null;
            }

            elapsedFlashTime = 0f;

            // �t�F�[�h�A�E�g����
            while (elapsedFlashTime < fadeDuration)
            {
                float alpha = 1f - (elapsedFlashTime / fadeDuration);
                flashImage.color = new Color(1f, 1f, 1f, alpha);
                elapsedFlashTime += Time.deltaTime;
                yield return null;
            }

            flashImage.gameObject.SetActive(false); // �t���b�V���C���[�W���\��
        }
    }

    // �I�u�W�F�N�g���폜���A���X�g������폜����
    void DestroyAndRemoveFromList()
    {
        // �I�u�W�F�N�g��j��
        Destroy(gameObject); // �q�I�u�W�F�N�g���폜
    }
}

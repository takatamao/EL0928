using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private InputAction _anyButtonAction;

    [SerializeField,Header("�J�ڐ�"),Tooltip("string,�J�ڐ�̃V�[���������")]
    private string nextSceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void Awake()
    {
        //action�쐬
        _anyButtonAction = new InputAction(
            "AnyButton",
            InputActionType.Button
            );

        //�L�[�{�[�h�̔C�ӃL�[���o�C���h
        _anyButtonAction.AddBinding("<Keyboard>/anyKey");

        //�R�[���o�b�N�o�^
        _anyButtonAction.performed += OnAnyButton;
    }

    private void OnDestroy()
    {
        //�R�[���o�b�N����
        _anyButtonAction.performed -= OnAnyButton;

        //action�̔j��
        _anyButtonAction.Dispose();
    }

    private void OnEnable()
    {
        //action�̗L����
        _anyButtonAction.Enable();
    }

    private void OnDisable()
    {
        //action�̖�����
        _anyButtonAction.Disable();
    }

    private void OnAnyButton(InputAction.CallbackContext context)
    {
        //�J�ڎ���SE��}��

        SceneManager.LoadScene(nextSceneName);
    }
}

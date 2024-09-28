using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private InputAction _anyButtonAction;

    [SerializeField,Header("遷移先"),Tooltip("string,遷移先のシーン名を入力")]
    private string nextSceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void Awake()
    {
        //action作成
        _anyButtonAction = new InputAction(
            "AnyButton",
            InputActionType.Button
            );

        //キーボードの任意キーをバインド
        _anyButtonAction.AddBinding("<Keyboard>/anyKey");

        //コールバック登録
        _anyButtonAction.performed += OnAnyButton;
    }

    private void OnDestroy()
    {
        //コールバック解除
        _anyButtonAction.performed -= OnAnyButton;

        //actionの破棄
        _anyButtonAction.Dispose();
    }

    private void OnEnable()
    {
        //actionの有効化
        _anyButtonAction.Enable();
    }

    private void OnDisable()
    {
        //actionの無効化
        _anyButtonAction.Disable();
    }

    private void OnAnyButton(InputAction.CallbackContext context)
    {
        //遷移時のSEを挿入

        SceneManager.LoadScene(nextSceneName);
    }
}

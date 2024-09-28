using UnityEngine;
using System.Collections.Generic;

public class debugCutin : MonoBehaviour
{
    public float jumpForce = 30f;       // ジャンプ力を上げる
    public float gravityScale = 3f;     // 重力スケールを調整して落下速度を上げる
    public float lifetime = 10f;        // オブジェクトが消えるまでの時間（秒）
    public AudioClip jumpSound;         // 効果音のAudioClip
    private AudioSource audioSource;    // AudioSourceコンポーネントを格納する変数

    void Start()
    {
        // オーディオソースを取得
        audioSource = GetComponent<AudioSource>();

        // 効果音を再生
        if (jumpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }

        // 開始時に消滅タイマーをセットし、削除時にリストから消去
        Invoke("DestroyAndRemoveFromList", lifetime);

        // Rigidbody2Dの重力スケールを設定
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = gravityScale; // 重力スケールを変更
        }

        // 1度だけジャンプを実行
        Jump();
    }

    // ジャンプ処理メソッド
    void Jump()
    {
        // ジャンプ力をY軸方向に加える
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // ジャンプ力をY軸方向に設定
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else
        {
            Debug.LogWarning("Rigidbody2Dがアタッチされていません。");
        }
    }

    // オブジェクトを削除し、リストからも削除する
    void DestroyAndRemoveFromList()
    {
        

        // 親オブジェクトを取得
        Transform parentTransform = transform.parent;

        // オブジェクトを破棄
        Destroy(gameObject); // 子オブジェクトを削除

        // 親オブジェクトも削除
        if (parentTransform != null)
        {
            Destroy(parentTransform.gameObject);
        }
    }

}

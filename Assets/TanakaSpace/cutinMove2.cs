using UnityEngine;
using UnityEngine.UI; // UIを使用するために追加
using System.Collections; // IEnumeratorを使用するために追加

public class CutinMove2 : MonoBehaviour
{
    public float lifetime = 1f; // オブジェクトが消えるまでの時間（秒）
    public AudioClip moveSound; // 効果音のAudioClip
    private AudioSource audioSource; // AudioSourceコンポーネントを格納する変数

    public float scaleUpDuration = 0.1f; // スケールが0から1.5までの時間
    public float scaleHoldDuration = 1f; // スケールが維持される時間
    public float scaleDownDuration = 0.3f; // スケールが1.5から元のサイズまでの時間
    public Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f); // 最終的なスケール
    private Vector3 initialScale; // 初期スケール
    private float elapsedTime = 0f; // 経過時間
    private bool scalingUp = true; // 現在スケールを拡大中かどうか
    private bool holding = false; // スケールを維持しているかどう

    public float moveSpeed = 100f; // オブジェクトの移動速度

    // フラッシュエフェクト用のUIイメージ
    public Image flashImage; // UIイメージを格納する変数
    private bool isFlashing = false; // フラッシュ中かどうか

    void Start()
    {
        // オーディオソースを取得
        audioSource = GetComponent<AudioSource>();

        // 効果音を再生
        if (moveSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(moveSound);
        }

        // 初期スケールを保存
        initialScale = transform.localScale;

        // 開始時に消滅タイマーをセットし、削除時にリストから消去
        Invoke("DestroyAndRemoveFromList", lifetime);
    }

    void Update()
    {
        // スケールの補完処理
        if (scalingUp)
        {
            if (elapsedTime < scaleUpDuration)
            {
                elapsedTime += Time.deltaTime; // 経過時間を増加させる
                float t = elapsedTime / scaleUpDuration; // 線形補完の進行度
                transform.localScale = Vector3.Lerp(initialScale, targetScale, t); // スケールを補完
            }
            else
            {
                // スケールの拡大が完了したら、保持時間に遷移
                scalingUp = false;
                holding = true; // スケールを維持する状態に
                elapsedTime = 0f; // 経過時間をリセット

                // フラッシュを開始
                StartCoroutine(FlashScreen());
            }
        }
        else if (holding)
        {
            if (elapsedTime < scaleHoldDuration)
            {
                elapsedTime += Time.deltaTime; // 維持時間の経過を増加させる
            }
            else
            {
                // 維持時間が経過したら、スケールダウンを開始
                holding = false;
                elapsedTime = 0f; // 経過時間をリセット
            }
        }
        else
        {
            if (elapsedTime < scaleDownDuration)
            {
                elapsedTime += Time.deltaTime; // 経過時間を増加させる
                float t = elapsedTime / scaleDownDuration; // 線形補完の進行度
                transform.localScale = Vector3.Lerp(targetScale, initialScale, t); // スケールを補完

                // スケールが0以下になったら削除
                if (transform.localScale.x <= 0f || transform.localScale.y <= 0f || transform.localScale.z <= 0f)
                {
                    DestroyAndRemoveFromList();
                }
            }
            else
            {
                // スケールダウンが完了した場合は削除
                DestroyAndRemoveFromList();
            }
        }
    }

    private IEnumerator FlashScreen()
    {
        if (flashImage != null)
        {
            flashImage.gameObject.SetActive(true); // フラッシュイメージを表示
            flashImage.color = new Color(1f, 1f, 1f, 1f); // 白色で不透明に設定
            float flashDuration = 0.01f; // フラッシュの持続時間
            float fadeDuration = 0.1f; // フラッシュから透明に戻る時間
            float elapsedFlashTime = 0f;

            // フラッシュのフェードアウト
            while (elapsedFlashTime < flashDuration)
            {
                elapsedFlashTime += Time.deltaTime;
                yield return null;
            }

            elapsedFlashTime = 0f;

            // フェードアウト処理
            while (elapsedFlashTime < fadeDuration)
            {
                float alpha = 1f - (elapsedFlashTime / fadeDuration);
                flashImage.color = new Color(1f, 1f, 1f, alpha);
                elapsedFlashTime += Time.deltaTime;
                yield return null;
            }

            flashImage.gameObject.SetActive(false); // フラッシュイメージを非表示
        }
    }

    // オブジェクトを削除し、リストからも削除する
    void DestroyAndRemoveFromList()
    {
        // オブジェクトを破棄
        Destroy(gameObject); // 子オブジェクトを削除
    }
}

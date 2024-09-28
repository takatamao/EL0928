using UnityEngine;

public class SomeOtherClass : MonoBehaviour
{
    public CutinAppearance cutinAppearance; // CutinAppearanceコンポーネントへの参照

    void Update()
    {
        // スペースキーが押されたらプレハブを生成
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (cutinAppearance != null)
            {
                cutinAppearance.SpawnPrefab(0); // 指定したインデックスのプレハブを生成
            }
        }
    }
}

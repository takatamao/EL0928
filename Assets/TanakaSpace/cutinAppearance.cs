using UnityEngine;
using System.Collections.Generic;

public class CutinAppearance : MonoBehaviour
{
    public List<GameObject> prefabList; // インスペクターから複数のプレハブを指定できるようにする
    public Vector3 spawnPosition;       // プレハブを出現させる座標

    public void SpawnPrefab(int index)
    {
        if (index >= 0 && index < prefabList.Count)
        {
            GameObject selectedPrefab = prefabList[index];
            Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Invalid prefab index: {index}. Valid range is 0 to {prefabList.Count - 1}.");
        }
    }

}

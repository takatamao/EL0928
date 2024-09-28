using UnityEngine;
using System.Collections.Generic;

public class CutinAppearance : MonoBehaviour
{
    public List<GameObject> prefabList; // �C���X�y�N�^�[���畡���̃v���n�u���w��ł���悤�ɂ���
    public Vector3 spawnPosition;       // �v���n�u���o����������W

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

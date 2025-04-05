using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameObject[] dogPrefabs; // 小狗预制体数组

    private void Start()
    {
        // 获取玩家选择的小狗品种和MBTI类型
        int selectedType = DogSelectionManager.Instance.selectedDogType;
        string selectedMBTI = DogSelectionManager.Instance.selectedMBTI;

        // 检查选择是否有效
        if (selectedType >= 0 && selectedType < dogPrefabs.Length)
        {
            // 实例化玩家选择的小狗
            GameObject dog = Instantiate(dogPrefabs[selectedType], Vector3.zero, Quaternion.identity);
            dog.name = selectedMBTI; // 设置小狗的名字为MBTI类型
            Debug.Log("Loaded Dog: " + dog.name);
        }
        else
        {
            Debug.LogError("Invalid dog selection!");
        }
    }
}
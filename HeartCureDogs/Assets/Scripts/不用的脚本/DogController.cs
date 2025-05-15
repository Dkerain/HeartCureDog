using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    public GameObject[] dogPrefabs; // 小狗预制体数组
    public Transform spawnPoint; // 小狗出现的位置
    public GameObject blanket; // 毛毯对象

    public GameObject dogInstance; // 小狗实例的引用

    private void Start()
    {
        // 加载小狗
        int selectedBreed = PlayerPrefs.GetInt("SelectedBreed", 0);
        if (selectedBreed >= 0 && selectedBreed < dogPrefabs.Length)
        {
            dogInstance = Instantiate(dogPrefabs[selectedBreed], spawnPoint.position, Quaternion.identity);
            dogInstance.transform.SetParent(blanket.transform);

            // 获取 DogMovement 组件并禁用移动
            DogMovement dogMovement = dogInstance.GetComponent<DogMovement>();
            if (dogMovement != null)
            {
                dogMovement.DisableMovement();
            }
        }
        else
        {
            Debug.LogError("小狗预制体索引超出范围！");
        }
    }

    public void EnableDogMovement()
    {
        if (dogInstance != null)
        {
            DogMovement dogMovement = dogInstance.GetComponent<DogMovement>();
            if (dogMovement != null)
            {
                dogMovement.EnableFreeMovement();
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInitializer : MonoBehaviour
{
    [Header("全局管理器预制体")]
    public GameObject saveManagerPrefab;

    void Awake()
    {
        InitializeGlobalManagers();
    }

    private void InitializeGlobalManagers()
    {
        // 确保 SaveManager 存在
        if (SaveManager.Instance == null)
        {
            Debug.Log("创建 SaveManager");

            if (saveManagerPrefab != null)
            {
                Instantiate(saveManagerPrefab);
            }
            else
            {
                // 备用方案：动态创建
                GameObject saveManagerObj = new GameObject("SaveManager");
                saveManagerObj.AddComponent<SaveManager>();
            }
        }

        // 可以在这里添加其他全局管理器...
    }
}

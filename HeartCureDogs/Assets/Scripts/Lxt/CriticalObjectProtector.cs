using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalObjectProtector : MonoBehaviour
{
    [Header("需要保护的对象")]
    public GameObject mainCamera;
    public GameObject mainCanvas;

    void Update()
    {
        // 持续监控关键对象状态
        if (mainCamera != null && !mainCamera.activeInHierarchy)
        {
            Debug.LogWarning("MainCamera 被意外禁用，正在恢复...");
            mainCamera.SetActive(true);
        }

        if (mainCanvas != null && !mainCanvas.activeInHierarchy)
        {
            Debug.LogWarning("MainCanvas 被意外禁用，正在恢复...");
            mainCanvas.SetActive(true);
        }
    }

    void OnEnable()
    {
        // 场景加载时确保关键对象激活
        EnsureCriticalObjectsActive();
    }

    private void EnsureCriticalObjectsActive()
    {
        if (mainCamera == null)
            mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCanvas == null)
            mainCanvas = GameObject.Find("MainCanvas"); // 根据实际名称调整

        if (mainCamera != null) mainCamera.SetActive(true);
        if (mainCanvas != null) mainCanvas.SetActive(true);
    }
}

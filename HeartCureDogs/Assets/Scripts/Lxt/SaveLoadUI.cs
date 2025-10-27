using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SaveLoadUI : MonoBehaviour
{
    [Header("存档按钮")]
    public Button[] saveButtons;
    public TextMeshProUGUI[] saveTexts;

    [Header("读档按钮")]
    public Button[] loadButtons;
    public TextMeshProUGUI[] loadTexts;

    [Header("界面控制")]
    public GameObject savePanel;
    public GameObject loadPanel;

    void Start()
    {
        Debug.Log("SaveLoadUI Start 开始初始化");
        InitializeUI();
    }

    private void InitializeUI()
    {
        Debug.Log($"初始化UI: 存档按钮{saveButtons.Length}个, 读档按钮{loadButtons.Length}个");
        //初始化存档界面按钮
        for (int i = 0; i < saveButtons.Length; i++)
        {
            int slotIndex = i; // 重要：创建局部变量
            if(saveButtons[i] != null)
            {
                saveButtons[i].onClick.RemoveAllListeners();
                saveButtons[i].onClick.AddListener(()=>OnSaveButtonClicked(slotIndex));
                Debug.Log($"初始化存档按钮 {i}");
            }
        }
        // 初始化读档界面按钮
        for (int i = 0; i < loadButtons.Length; i++)
        {
            int slotIndex = i;
            if (loadButtons[i] != null)
            {
                loadButtons[i].onClick.RemoveAllListeners();
                loadButtons[i].onClick.AddListener(() => OnLoadButtonClicked(slotIndex));
                Debug.Log($"初始化读档按钮 {i}");
            }
        }
    }
    // 显示存档界面
    public void ShowSavePanel()
    {
        Debug.Log("显示存档界面");
        savePanel.SetActive(true);
        loadPanel.SetActive(false);
        RefreshSaveDisplays();
    }

    // 显示读档界面
    public void ShowLoadPanel()
    {
        Debug.Log("显示读档界面");
        savePanel.SetActive(false);
        loadPanel.SetActive(true);
        RefreshLoadDisplays();
    }
    /*private System.Collections.IEnumerator DelayedRefreshSavePanel()
    {
        yield return new WaitForEndOfFrame();
        RefreshSaveDisplays();
    }*/

    //存档按钮点击事件
    private void OnSaveButtonClicked(int slotIndex)
    {
        Debug.Log($"点击了存档按钮 {slotIndex}");

        // 在保存前检查关键对象状态
        CheckCriticalObjects("保存存档前");

        SaveManager.Instance.SaveGame(slotIndex);
        RefreshSaveDisplays();

        // 确保关键对象保持激活
        EnsureCriticalObjectsActive();

        Debug.Log("存档完成");
        /*//if (savePanel.activeInHierarchy)
        {
            // 存档模式
            Debug.Log($"存档到位置{slotIndex}");
            SaveManager.Instance.SaveGame(slotIndex);
            StartCoroutine(DelayedRefresh());  
            //RefreshSaveDisplays();//立即刷新存档界面显示
        }*/
        /*else if (loadPanel.activeInHierarchy)
        {
            // 读档模式
            if (!SaveManager.Instance.IsSaveSlotEmpty(slotIndex))
            {
                SaveManager.Instance.LoadGame(slotIndex);
                // 可以在这里关闭界面或加载场景
                gameObject.SetActive(false);
            }
        }*/
    }
    private System.Collections.IEnumerator DelayedRefresh()
    {
        yield return new WaitForEndOfFrame();
        RefreshSaveDisplays();
        Debug.Log("延迟刷新完成");
    }

    // 读档按钮点击事件
    private void OnLoadButtonClicked(int slotIndex)
    {
        Debug.Log($"点击了读档按钮 {slotIndex}");
        /*if (!SaveManager.Instance.IsSaveSlotEmpty(slotIndex))
        {
            SaveManager.Instance.LoadGame(slotIndex);
            // 读档后可以关闭界面或进行其他操作
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("该存档位为空");
        }*/
        // 在加载前检查关键对象状态
        CheckCriticalObjects("加载存档前");

        if (!SaveManager.Instance.IsSaveSlotEmpty(slotIndex))
        {
            SaveManager.Instance.LoadGame(slotIndex);

            // 确保关键对象保持激活
            EnsureCriticalObjectsActive();

            // 只关闭存档界面，不影响其他对象
            loadPanel.SetActive(false);

            Debug.Log("读档完成，仅关闭读档界面");
        }
        else
        {
            Debug.Log("该存档位为空");
        }
    }
    // 检查关键对象状态
    private void CheckCriticalObjects(string context)
    {
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        GameObject mainCanvas = GameObject.Find("MainCanvas"); // 根据你的实际名称调整

        Debug.Log($"{context} - MainCamera: {mainCamera?.activeInHierarchy}, MainCanvas: {mainCanvas?.activeInHierarchy}");
    }
    // 确保关键对象激活
    private void EnsureCriticalObjectsActive()
    {
        // 确保主相机激活
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null && !mainCamera.activeInHierarchy)
        {
            mainCamera.SetActive(true);
            Debug.LogWarning("已重新激活 MainCamera");
        }

        // 确保主画布激活
        GameObject mainCanvas = GameObject.Find("MainCanvas"); // 根据你的实际名称调整
        if (mainCanvas != null && !mainCanvas.activeInHierarchy)
        {
            mainCanvas.SetActive(true);
            Debug.LogWarning("已重新激活 MainCanvas");
        }

        // 确保至少有一个相机在渲染
        if (Camera.main == null)
        {
            Debug.LogError("没有激活的主相机！");
            // 紧急恢复：激活找到的第一个相机
            Camera[] cameras = FindObjectsOfType<Camera>(true); // 包括未激活的
            if (cameras.Length > 0)
            {
                cameras[0].gameObject.SetActive(true);
                cameras[0].tag = "MainCamera";
                Debug.LogWarning($"紧急激活相机: {cameras[0].name}");
            }
        }
    }
    //刷新存档界面显示
    private void RefreshSaveDisplays()
    {
        Debug.Log("刷新存档界面显示");
        for (int i = 0; i < saveTexts.Length; i++)
        {
            if (saveTexts[i] != null)
            {
                //saveTexts[i].text = SaveManager.Instance.GetSaveDisplayText(i);
                string displayText = SaveManager.Instance.GetSaveDisplayText(i);
                saveTexts[i].text = displayText;
                Debug.Log($"存档界面按钮 {i} 文本设置为: {displayText}");
            }
        }
    }
    // 刷新读档界面显示
    private void RefreshLoadDisplays()
    {
        Debug.Log("刷新读档界面显示");
        for (int i = 0; i < loadTexts.Length; i++)
        {
            if (loadTexts[i] != null)
            {
                string displayText = SaveManager.Instance.GetSaveDisplayText(i);
                loadTexts[i].text = displayText;
                Debug.Log($"读档界面按钮 {i} 文本设置为: {displayText}");
                //loadTexts[i].text = SaveManager.Instance.GetSaveDisplayText(i);
            }
        }
    }
    // 统一刷新所有界面（可选）
    public void RefreshAllDisplays()
    {
        RefreshSaveDisplays();
        RefreshLoadDisplays();
    }
    // 在编辑器中测试用的方法
    [ContextMenu("测试刷新存档显示")]
    public void TestRefresh()
    {
        RefreshSaveDisplays();
        RefreshLoadDisplays();
    }
}

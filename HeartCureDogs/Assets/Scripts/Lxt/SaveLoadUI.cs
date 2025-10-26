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
        InitializeUI();
    }

    private void InitializeUI()
    {
        //初始化存档界面按钮
        for (int i = 0; i < saveButtons.Length; i++)
        {
            int slotIndex = i; // 重要：创建局部变量
            if(saveButtons[i] != null)
            {
                saveButtons[i].onClick.RemoveAllListeners();
                saveButtons[i].onClick.AddListener(()=>OnSaveButtonClicked(slotIndex));
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
            }
        }
    }
    // 显示存档界面
    public void ShowSavePanel()
    {
        savePanel.SetActive(true);
        loadPanel.SetActive(false);
        RefreshSaveDisplays();
    }

    // 显示读档界面
    public void ShowLoadPanel()
    {
        savePanel.SetActive(false);
        loadPanel.SetActive(true);
        RefreshSaveDisplays();
    }

    //存档按钮点击事件
    private void OnSaveButtonClicked(int slotIndex)
    {
        //if (savePanel.activeInHierarchy)
        {
            // 存档模式
            Debug.Log($"存档到位置{slotIndex}");
            SaveManager.Instance.SaveGame(slotIndex);
            RefreshSaveDisplays();//立即刷新存档界面显示
        }
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
    // 读档按钮点击事件
    private void OnLoadButtonClicked(int slotIndex)
    {
        Debug.Log($"从位置 {slotIndex} 读档");
        if (!SaveManager.Instance.IsSaveSlotEmpty(slotIndex))
        {
            SaveManager.Instance.LoadGame(slotIndex);
            // 读档后可以关闭界面或进行其他操作
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("该存档位为空");
        }
    }
    //刷新存档界面显示
    private void RefreshSaveDisplays()
    {
        for (int i = 0; i < saveTexts.Length; i++)
        {
            if (saveTexts[i] != null)
            {
                saveTexts[i].text = SaveManager.Instance.GetSaveDisplayText(i);
            }
        }
    }
    // 刷新读档界面显示
    private void RefreshLoadDisplays()
    {
        for (int i = 0; i < loadTexts.Length; i++)
        {
            if (loadTexts[i] != null)
            {
                loadTexts[i].text = SaveManager.Instance.GetSaveDisplayText(i);
            }
        }
    }
    // 统一刷新所有界面（可选）
    public void RefreshAllDisplays()
    {
        RefreshSaveDisplays();
        RefreshLoadDisplays();
    }
}

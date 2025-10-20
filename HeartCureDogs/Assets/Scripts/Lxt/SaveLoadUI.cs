using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveLoadUI : MonoBehaviour
{
    [Header("存档按钮")]
    public Button[] saveButtons;
    public TextMeshProUGUI[] saveTexts;

    [Header("界面控制")]
    public GameObject savePanel;
    public GameObject loadPanel;

    void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        // 为每个存档按钮添加监听
        for (int i = 0; i < saveButtons.Length; i++)
        {
            int slotIndex = i; // 重要：创建局部变量
            saveButtons[i].onClick.AddListener(() => OnSaveButtonClicked(slotIndex));
        }

        RefreshSaveDisplays();
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

    private void OnSaveButtonClicked(int slotIndex)
    {
        if (savePanel.activeInHierarchy)
        {
            // 存档模式
            SaveManager.Instance.SaveGame(slotIndex);
            RefreshSaveDisplays();
        }
        else if (loadPanel.activeInHierarchy)
        {
            // 读档模式
            if (!SaveManager.Instance.IsSaveSlotEmpty(slotIndex))
            {
                SaveManager.Instance.LoadGame(slotIndex);
                // 可以在这里关闭界面或加载场景
                gameObject.SetActive(false);
            }
        }
    }

    private void RefreshSaveDisplays()
    {
        for (int i = 0; i < saveTexts.Length; i++)
        {
            saveTexts[i].text = SaveManager.Instance.GetSaveDisplayText(i);
        }
    }
}

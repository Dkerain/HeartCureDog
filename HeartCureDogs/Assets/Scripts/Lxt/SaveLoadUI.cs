using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;


public class SaveLoadUI : MonoBehaviour
{
    //private System.Action onCloseCallback;
    [Header("界面引用")]
    public Button[] saveButtons;
    public TextMeshProUGUI[] saveTexts;
    public Button[] loadButtons;
    public TextMeshProUGUI[] loadTexts;
    public GameObject savePanel;
    public GameObject loadPanel;

    //[Header("预制体设置")]
    //public static SaveLoadUI Instance;
    [Header("回调设置")]
    private System.Action onCloseCallback;

    public static SaveLoadUI Instance;
    private bool isLoadMenu; // true=读档界面, false=存档界面

    /*
    [Header("存档按钮")]
    public Button[] saveButtons;
    public TextMeshProUGUI[] saveTexts;

    [Header("读档按钮")]
    public Button[] loadButtons;
    public TextMeshProUGUI[] loadTexts;

    [Header("界面控制")]
    public GameObject savePanel;
    public GameObject loadPanel;
    */

    // 静态方法用于创建读档界面
    public static void CreateLoadMenu(Transform parent = null, System.Action onClose = null)
    {
        // 如果已经存在，直接显示
        if (Instance != null)
        {
            Instance.ShowLoadPanel();
            Instance.onCloseCallback = onClose;
            return;
        }

        // 动态加载预制体
        GameObject loadCanvasPrefab = Resources.Load<GameObject>("Prefabs/LoadCanvas");
        if (loadCanvasPrefab != null)
        {
            GameObject loadCanvas = Instantiate(loadCanvasPrefab, parent);
            Instance = loadCanvas.GetComponent<SaveLoadUI>();
            Instance.isLoadMenu = true;
            Instance.onCloseCallback = onClose;
            Instance.ShowLoadPanel();
            Debug.Log("读档界面创建完成");
        }
        else
        {
            Debug.LogError("LoadCanvas 预制体未找到！请确保路径为 Resources/Prefabs/LoadCanvas");
        }
    }

    // 静态方法用于创建存档界面
    public static void CreateSaveMenu(Transform parent = null,System.Action onClose=null)
    {
        if (Instance != null)
        {
            Instance.ShowSavePanel();
            Instance.onCloseCallback = onClose;
            return;
        }

        GameObject saveCanvasPrefab = Resources.Load<GameObject>("Prefabs/SaveCanvas");
        if (saveCanvasPrefab != null)
        {
            GameObject saveCanvas = Instantiate(saveCanvasPrefab, parent);
            Instance = saveCanvas.GetComponent<SaveLoadUI>();
            Instance.isLoadMenu = false;
            Instance.onCloseCallback = onClose;
            Instance.ShowSavePanel();
            Debug.Log("存档界面创建完成");
        }
        else
        {
            Debug.LogError("LoadCanvas 预制体未找到！");
        }
    }
    void Start()
    {
        Debug.Log("SaveLoadUI Start 开始初始化");
        InitializeUI();
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 不要使用 DontDestroyOnLoad，我们想要每个场景独立控制
            InitializeUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu(); 
        }
    }
    // 添加关闭方法
    public void CloseMenu()
    {
        Debug.Log("关闭存档/读档界面");
        onCloseCallback?.Invoke();
        // 如果是动态创建的，销毁；如果是场景中的，只是隐藏
        if (IsDynamicallyCreated())
        {
            Destroy(gameObject);
            Instance = null;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public bool IsDynamicallyCreated()
    {
        // 检查这个对象是否来自预制体实例
        return gameObject.scene.name == null;
    }
  

    private void InitializeUI()
    {
        Debug.Log("初始化SaveLoadUI界面");

        // 检查关键引用
        CheckReferences();

        // 初始化存档界面按钮（只有savePanelButtons不为空时才初始化）
        if (saveButtons != null && saveButtons.Length > 0)
        {
            for (int i = 0; i < saveButtons.Length; i++)
            {
                int slotIndex = i;
                if (saveButtons[i] != null)
                {
                    saveButtons[i].onClick.RemoveAllListeners();
                    saveButtons[i].onClick.AddListener(() => OnSaveButtonClicked(slotIndex));
                    Debug.Log($"初始化存档按钮 {i}");
                }
            }
        }

        // 初始化读档界面按钮（只有loadPanelButtons不为空时才初始化）
        if (loadButtons != null && loadButtons.Length > 0)
        {
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

        // 初始刷新显示
        RefreshAllDisplays();
    }
    // 检查所有引用是否设置
    private void CheckReferences()
    {
        Debug.Log("检查SaveLoadUI引用...");
        Debug.Log($"savePanel: {savePanel != null}");
        Debug.Log($"loadPanel: {loadPanel != null}");
        Debug.Log($"savePanelButtons: {saveButtons != null} (长度: {saveButtons?.Length})");
        Debug.Log($"loadPanelButtons: {loadButtons != null} (长度: {loadButtons?.Length})");
        Debug.Log($"savePanelTexts: {saveTexts != null} (长度: {saveTexts?.Length})");
        Debug.Log($"loadPanelTexts: {loadTexts != null} (长度: {loadTexts?.Length})");
    }

    // 显示存档界面
    public void ShowSavePanel()
    {
        Debug.Log("显示存档界面");
        // 检查数组引用
        Debug.Log($"loadPanelTexts 数组: {(loadTexts == null ? "null" : "not null")}");
        // 检查引用
        if (savePanel == null)
        {
            Debug.LogError("savePanel 未设置！");
            return;
        }
        if (loadTexts != null)
        {
            Debug.Log($"loadPanelTexts 长度: {loadTexts.Length}");
            for (int i = 0; i < loadTexts.Length; i++)
            {
                Debug.Log($"loadPanelTexts[{i}]: {(loadTexts[i] == null ? "null" : loadTexts[i].name)}");
            }
        }
        else
        {
            Debug.LogError("loadPanelTexts 数组为 null!");
        }
        savePanel.SetActive(true);
        if (loadPanel != null) loadPanel.SetActive(false);
        RefreshSaveDisplays();
        StartCoroutine(DelayedRefreshSavePanel());
    }

    // 显示读档界面
    public void ShowLoadPanel()
    {
        Debug.Log("显示读档界面");
        // 检查引用
        if (loadPanel == null)
        {
            Debug.LogError("loadPanel 未设置！");
            return;
        }
        if(savePanel != null) savePanel.SetActive(false);
        loadPanel.SetActive(true);
        RefreshLoadDisplays();
        StartCoroutine(DelayedRefreshLoadPanel());
    }
    private IEnumerator DelayedRefreshSavePanel()
    {
        yield return new WaitForEndOfFrame();
        RefreshSaveDisplays();
    }

    private IEnumerator DelayedRefreshLoadPanel()
    {
        yield return new WaitForEndOfFrame();
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
            // 读档后延迟关闭界面并加载游戏场景
            StartCoroutine(LoadGameAfterDelay());

            Debug.Log("读档完成，仅关闭读档界面");
        }
        else
        {
            Debug.Log("该存档位为空");
        }
    }
    private System.Collections.IEnumerator LoadGameAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // 短暂延迟让存档加载完成
        CloseMenu();

        // 加载游戏场景
        //SceneManager.LoadScene("GameScene");
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
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
        // 检查数组是否为空
        if (saveTexts == null || saveTexts.Length == 0)
        {
            Debug.LogWarning("savePanelTexts 数组为空，无法刷新存档显示");
            return;
        }
        for (int i = 0; i < saveTexts.Length; i++)
        {
            if (saveTexts[i] != null)
            {
                //saveTexts[i].text = SaveManager.Instance.GetSaveDisplayText(i);
                string displayText = SaveManager.Instance.GetSaveDisplayText(i);
                saveTexts[i].text = displayText;
                Debug.Log($"存档位 {i}: Text组件={saveTexts[i].name}, 文本内容='{displayText}'");
                Debug.Log($"Text组件激活状态: {saveTexts[i].gameObject.activeInHierarchy}");
                Debug.Log($"Text组件启用状态: {saveTexts[i].enabled}");
                Debug.Log($"存档界面按钮 {i} 文本设置为: {displayText}");
            }
            else
            {
                Debug.LogError($"存档位 {i}: savePanelTexts 为 null!");
            }
        }
        Debug.Log("=== 结束刷新存档界面显示 ===");
    }
    // 刷新读档界面显示
    private void RefreshLoadDisplays()
    {
        Debug.Log("===开始刷新读档界面显示===");
        // 检查数组是否为空
        if (loadTexts == null || loadTexts.Length == 0)
        {
            Debug.LogWarning("loadPanelTexts 数组为空，无法刷新读档显示");
            return;
        }

        for (int i = 0; i < loadTexts.Length; i++)
        {
            if (loadTexts[i] != null)
            {
                string displayText = SaveManager.Instance.GetSaveDisplayText(i);
                loadTexts[i].text = displayText;
                //Debug.Log($"读档界面按钮 {i} 文本设置为: {displayText}");
                Debug.Log($"读档位 {i}: Text组件={loadTexts[i].name}, 文本内容='{displayText}'");
                Debug.Log($"Text组件激活状态: {loadTexts[i].gameObject.activeInHierarchy}");
                Debug.Log($"Text组件启用状态: {loadTexts[i].enabled}");
                //loadTexts[i].text = SaveManager.Instance.GetSaveDisplayText(i);
                // 强制刷新 UI 布局
                Canvas.ForceUpdateCanvases();
                LayoutRebuilder.ForceRebuildLayoutImmediate(loadTexts[i].transform as RectTransform);
                Debug.Log($"读档位 {i}: 文本已设置并强制刷新");
            }
            else
            {
                Debug.LogError($"读档位{i}:saveTexts为null");
            }
        }
        // 整体刷新画布
        Canvas.ForceUpdateCanvases();
        Debug.Log("===结束刷新读档界面显示===");
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
    [ContextMenu("手动刷新显示")]
    public void ManualRefresh()
    {
        RefreshAllDisplays();
    }
    [ContextMenu("自动查找引用")]
    public void AutoFindReferences()
    {
        Debug.Log("开始自动查找引用...");

        // 自动查找面板
        if (savePanel == null)
        {
            Transform savePanelTransform = transform.Find("SavePanel");
            if (savePanelTransform != null)
            {
                savePanel = savePanelTransform.gameObject;
                Debug.Log($"找到 savePanel: {savePanel.name}");
            }
        }

        if (loadPanel == null)
        {
            Transform loadPanelTransform = transform.Find("LoadPanel");
            if (loadPanelTransform != null)
            {
                loadPanel = loadPanelTransform.gameObject;
                Debug.Log($"找到 loadPanel: {loadPanel.name}");
            }
        }

        Debug.Log("自动查找完成");
    }
}

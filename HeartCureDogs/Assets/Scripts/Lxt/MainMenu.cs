using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("UI引用")]
    public GameObject mainMenuPanel;
    public Button startButton;
    public Button loadButton;
    public Button exitButton;
    [Header("其他UI")]
    public GameObject loadingPanel; // 可选的加载界面

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("MainMenu 启动，检查 SaveManager: " + (SaveManager.Instance != null));
        // 按钮事件绑定
        startButton.onClick.AddListener(OnStartGame);
        loadButton.onClick.AddListener(OnLoadGame);
        exitButton.onClick.AddListener(OnExitGame);

        // 隐藏主菜单以外的所有UI
        mainMenuPanel.SetActive(true);
        if (loadingPanel != null) loadingPanel.SetActive(false);

        // 清理可能残留的UI
        CleanupExistingSaveLoadUI();

        Debug.Log("主菜单初始化完成");
    }
    private void OnStartGame()
    {
        Debug.Log("开始新游戏");

        // 初始化新游戏数据
        InitializeNewGame();
        // 开始新游戏，设置初始章节
        /*Blackboard globalBlackboard = GlobalBlackboard.Find("Global");
        if (globalBlackboard != null)
        {
            globalBlackboard.SetValue("Chapter", 1); // 从第一章开始
        }*/

        // 异步加载游戏场景
        StartCoroutine(LoadGameScene("GameScene"));
    }

    private void OnLoadGame()
    {
        Debug.Log("打开读档界面");

        // 隐藏主菜单
        mainMenuPanel.SetActive(false);

        // 动态创建读档界面
        SaveLoadUI.CreateLoadMenu(transform, OnLoadMenuClosed);
    }

    private void OnExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // 当读档界面关闭时调用（可以在SaveLoadUI中回调）
    public void OnLoadMenuClosed()
    {
        Debug.Log("读档界面已关闭");
        // 重新显示主菜单
        mainMenuPanel.SetActive(true);
    }
    private void InitializeNewGame()
    {
        // 设置初始章节
        Blackboard globalBlackboard = GlobalBlackboard.Find("Global");
        if (globalBlackboard != null)
        {
            globalBlackboard.SetValue("Chapter", 1);
            Debug.Log("新游戏初始化：章节设置为1");
        }
        else
        {
            Debug.LogWarning("未找到全局黑板，无法初始化章节");
        }

        // 这里可以初始化其他游戏数据...
    }
    private IEnumerator LoadGameScene(string sceneName)
    {
        Debug.Log($"开始加载场景: {sceneName}");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // 模拟加载过程（可选）
        while (!asyncLoad.isDone)
        {
            // 可以在这里更新加载进度条
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        Debug.Log("场景加载完成");
    }
    private void CleanupExistingSaveLoadUI()
    {
        // 清理可能残留的SaveLoadUI实例
        SaveLoadUI existingUI = FindObjectOfType<SaveLoadUI>();
        if (existingUI != null && existingUI != SaveLoadUI.Instance)
        {
            Destroy(existingUI.gameObject);
            Debug.Log("清理残留的SaveLoadUI");
        }
    }
    void Update()
    {
        // 按ESC键返回主菜单（如果不在主菜单界面）
        if (Input.GetKeyDown(KeyCode.Escape) && !mainMenuPanel.activeInHierarchy)
        {
            // 如果存档/读档界面是动态创建的，关闭它
            if (SaveLoadUI.Instance != null && SaveLoadUI.Instance.IsDynamicallyCreated())
            {
                SaveLoadUI.Instance.CloseMenu();
                mainMenuPanel.SetActive(true);
            }
        }
    }
}

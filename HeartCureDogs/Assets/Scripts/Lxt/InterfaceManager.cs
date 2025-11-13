using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;

    private Stack<GameObject> interfaceStack = new Stack<GameObject>();

    [Header("手动指定主界面（可选）")]
    public List<SceneMainInterface> sceneMainInterfaces = new List<SceneMainInterface>();

    [Header("自动查找设置")]
    public string[] mainInterfaceNames = { "MainCanvas", "Canvas", "UIRoot", "UICanvas" };
    public bool debugMode = true;

    [System.Serializable]
    public class SceneMainInterface
    {
        public string sceneName;
        public GameObject mainInterface;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        // 初始场景的处理
        string currentScene = SceneManager.GetActiveScene().name;
        GameObject mainInterface = FindMainInterfaceForScene(currentScene);
        if (mainInterface != null)
        {
            PushInterface(mainInterface);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (debugMode) Debug.Log($"场景加载完成: {scene.name}");

        ClearStack();

        // 查找并压入新场景的主界面
        GameObject mainInterface = FindMainInterfaceForScene(scene.name);
        if (mainInterface != null)
        {
            PushInterface(mainInterface);
        }
        else
        {
            Debug.LogWarning($"在场景 {scene.name} 中未找到主界面");
        }
    }

    private GameObject FindMainInterfaceForScene(string sceneName)
    {
        // 1. 首先检查手动配置
        GameObject manualInterface = FindManualInterface(sceneName);
        if (manualInterface != null)
        {
            if (debugMode) Debug.Log($"通过手动配置找到界面: {manualInterface.name}");
            return manualInterface;
        }

        // 2. 自动查找
        GameObject autoInterface = AutoFindMainInterface();
        if (autoInterface != null)
        {
            if (debugMode) Debug.Log($"通过自动查找找到界面: {autoInterface.name}");
            return autoInterface;
        }

        // 3. 查找任何Canvas作为备选
        GameObject anyCanvas = FindAnyCanvas();
        if (anyCanvas != null)
        {
            if (debugMode) Debug.Log($"找到备选Canvas: {anyCanvas.name}");
            return anyCanvas;
        }

        return null;
    }

    private GameObject FindManualInterface(string sceneName)
    {
        foreach (var pair in sceneMainInterfaces)
        {
            if (pair.sceneName == sceneName && pair.mainInterface != null)
            {
                return pair.mainInterface;
            }
        }
        return null;
    }

    private GameObject AutoFindMainInterface()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();

        // 优先查找指定名称的对象
        foreach (string interfaceName in mainInterfaceNames)
        {
            foreach (GameObject obj in rootObjects)
            {
                if (obj.name == interfaceName)
                {
                    Canvas canvas = obj.GetComponent<Canvas>();
                    if (canvas != null) return obj;

                    // 如果没有Canvas组件，在子对象中查找
                    Canvas childCanvas = obj.GetComponentInChildren<Canvas>();
                    if (childCanvas != null) return obj;
                }
            }
        }

        // 查找任何包含Canvas的对象
        foreach (GameObject obj in rootObjects)
        {
            Canvas canvas = obj.GetComponentInChildren<Canvas>();
            if (canvas != null) return obj;
        }

        return null;
    }

    private GameObject FindAnyCanvas()
    {
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in allCanvases)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                return canvas.gameObject;
            }
        }

        // 返回找到的第一个Canvas
        if (allCanvases.Length > 0)
        {
            return allCanvases[0].gameObject;
        }

        return null;
    }

    // 原有的 PushInterface, PopInterface 等方法保持不变
    public void PushInterface(GameObject newInterface)
    {
        if (newInterface == null)
        {
            Debug.LogError("尝试压入空的界面！");
            return;
        }

        if (interfaceStack.Count > 0)
        {
            GameObject currentInterface = interfaceStack.Peek();
            if (currentInterface != null && currentInterface != newInterface)
            {
                currentInterface.SetActive(false);
                if (debugMode) Debug.Log("隐藏界面: " + currentInterface.name);
            }
        }

        newInterface.SetActive(true);
        interfaceStack.Push(newInterface);
        if (debugMode) Debug.Log("显示界面: " + newInterface.name + ", 当前栈大小: " + interfaceStack.Count);
    }

    public void PopInterface()
    {
        CleanDestroyedInterfaces();

        if (interfaceStack.Count < 2)
        {
            Debug.LogWarning($"界面栈太小无法返回。当前数量: {interfaceStack.Count}");
            return;
        }

        GameObject currentInterface = interfaceStack.Pop();
        if (currentInterface != null)
        {
            currentInterface.SetActive(false);
            if (debugMode) Debug.Log("关闭界面: " + currentInterface.name);
        }

        GameObject previousInterface = interfaceStack.Peek();
        if (previousInterface != null)
        {
            previousInterface.SetActive(true);
            if (debugMode) Debug.Log("打开界面: " + previousInterface.name);
        }
    }

    private void CleanDestroyedInterfaces()
    {
        Stack<GameObject> tempStack = new Stack<GameObject>();
        while (interfaceStack.Count > 0)
        {
            GameObject obj = interfaceStack.Pop();
            if (obj != null) tempStack.Push(obj);
        }
        while (tempStack.Count > 0) interfaceStack.Push(tempStack.Pop());
    }

    public void ClearStack()
    {
        interfaceStack.Clear();
        if (debugMode) Debug.Log("界面栈已清空");
    }

    [ContextMenu("打印栈信息")]
    public void PrintStackInfo()
    {
        Debug.Log($"=== 界面栈信息 ===");
        Debug.Log($"栈大小: {interfaceStack.Count}");
        int index = 0;
        foreach (var obj in interfaceStack)
        {
            string status = obj != null ? $"活跃 ({obj.name})" : "已销毁";
            Debug.Log($"[{index}] {status}");
            index++;
        }
    }

    [ContextMenu("查找当前场景主界面")]
    public void FindCurrentSceneInterface()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"=== 查找场景 {currentScene} 的主界面 ===");

        GameObject foundInterface = FindMainInterfaceForScene(currentScene);
        if (foundInterface != null)
        {
            Debug.Log($"找到主界面: {foundInterface.name}");
        }
        else
        {
            Debug.LogError("未找到主界面！");
        }
    }
}

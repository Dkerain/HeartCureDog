using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 飞盘接取小游戏管理器
/// 在ParkScene的GameCanvas中运行，控制游戏流程、生命值、飞盘生成等
/// </summary>
public class FrisbeeGame : MonoBehaviour
{
    [Header("游戏设置")]
    [Tooltip("初始生命值")]
    public int maxHearts = 3;
    [Tooltip("飞盘生成间隔（秒）")]
    public float frisbeeSpawnInterval = 2f;
    [Tooltip("难度递增系数（每次失误后调整）")]
    public float difficultyMultiplier = 1.05f;

    [Header("飞盘设置")]
    [Tooltip("飞盘预制体")]
    public GameObject frisbeePrefab;
    [Tooltip("飞盘飞行速度")]
    public float frisbeeSpeed = 5f;
    [Tooltip("飞盘生成位置（右边界）")]
    public float spawnXPosition = 10f;
    [Tooltip("飞盘Y轴随机高度范围")]
    public Vector2 spawnYRange = new Vector2(2f, 6f);

    [Header("狗与碰撞")]
    [Tooltip("小狗Transform")]
    public Transform dogTransform;
    [Tooltip("检测碰撞的距离（已弃用，使用碰撞器）")]
    public float catchDistance = 0.5f;

    [Header("UI")]
    [Tooltip("心脏UI容器")]
    public Transform heartsContainer;
    [Tooltip("单个心脏预制体")]
    public GameObject heartPrefab;
    [Tooltip("游戏结束时显示的面板")]
    public GameObject gameOverPanel;

    [Header("控制")]
    [Tooltip("狗的控制器（用来启用/禁用移动）")]
    public DogControllerr dogController;

    [Header("界面")]
    [Tooltip("主界面 Canvas（Game Button 所在）")]
    public GameObject mainCanvas;
    [Tooltip("小游戏 Canvas（GameCanvas）")]
    public GameObject gameCanvas;

    // 游戏状态
    private int currentHearts;
    private bool isGameActive = false;
    private List<GameObject> heartUIList = new List<GameObject>();
    private Coroutine spawnCoroutine;
    private float currentSpawnInterval;
    private float currentFrisbeeSpeed;

    void Start()
    {
        InitializeGame();
    }

    void Update()
    {
        if (isGameActive)
        {
            // 碰撞检测现在在Frisbee脚本中处理
            RemoveOffscreenFrisbees();
        }
    }

    /// <summary>
    /// 初始化游戏
    /// </summary>
    public void InitializeGame()
    {
        // 切换界面
        if (mainCanvas != null)
            mainCanvas.SetActive(false);
        if (gameCanvas != null)
            gameCanvas.SetActive(true);

        // 确保脚本组件激活
        this.enabled = true;

        // 清理所有旧飞盘
        Frisbee[] oldFrisbees = FindObjectsOfType<Frisbee>();
        foreach (Frisbee frisbee in oldFrisbees)
        {
            Destroy(frisbee.gameObject);
        }

        currentHearts = maxHearts;
        currentSpawnInterval = frisbeeSpawnInterval;
        currentFrisbeeSpeed = frisbeeSpeed;
        isGameActive = true;

        // 显示生命值UI
        UpdateHeartsUI();

        // 隐藏游戏结束面板
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // 启用狗的移动
        if (dogController != null)
            dogController.EnableMovement();

        // 停止之前的协程
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        // 直接启动飞盘生成协程
        spawnCoroutine = StartCoroutine(SpawnFrisbeesCoroutine());

        Debug.Log("游戏已初始化！飞盘生成协程已启动！");
    }

    /// <summary>
    /// 生成飞盘的协程
    /// </summary>
    private IEnumerator SpawnFrisbeesCoroutine()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(currentSpawnInterval);
            if (isGameActive)
            {
                SpawnFrisbee();
            }
        }
    }

    /// <summary>
    /// 生成一个飞盘
    /// </summary>
    private void SpawnFrisbee()
    {
        if (frisbeePrefab == null)
        {
            Debug.LogError("飞盘预制体未设置！");
            return;
        }

        // 随机Y位置
        float randomY = Random.Range(spawnYRange.x, spawnYRange.y);
        Vector3 spawnPos = new Vector3(spawnXPosition, randomY, 0);

        // 实例化飞盘
        GameObject frisbee = Instantiate(frisbeePrefab, spawnPos, Quaternion.identity);

        // 获取或添加飞盘脚本并设置速度和游戏管理器
        Frisbee frisbeeScript = frisbee.GetComponent<Frisbee>();
        if (frisbeeScript == null)
        {
            frisbeeScript = frisbee.AddComponent<Frisbee>();
        }
        frisbeeScript.SetSpeed(currentFrisbeeSpeed);
        frisbeeScript.SetGameManager(this);
        
        Debug.Log("生成了一个新飞盘！");
    }

    /// <summary>
    /// 飞盘被接住时的回调
    /// </summary>
    public void OnFrisbeeCaught()
    {
        // 增加难度
        currentSpawnInterval *= difficultyMultiplier;
        currentFrisbeeSpeed *= difficultyMultiplier;

        Debug.Log($"接住飞盘！难度提升 - 生成间隔: {currentSpawnInterval:F2}s, 速度: {currentFrisbeeSpeed:F2}");
    }

    /// <summary>
    /// 飞盘未被接住时的回调（从屏幕右侧消失）
    /// </summary>
    public void OnFrisbeeMissed()
    {
        currentHearts--;
        UpdateHeartsUI();

        Debug.Log($"未接住飞盘！剩余生命值: {currentHearts}");

        if (currentHearts <= 0)
        {
            GameOver();
        }
    }

    /// <summary>
    /// 更新心脏UI
    /// </summary>
    private void UpdateHeartsUI()
    {
        // 清空旧的心脏UI
        foreach (GameObject heart in heartUIList)
        {
            Destroy(heart);
        }
        heartUIList.Clear();

        // 创建新的心脏UI
        if (heartsContainer != null && heartPrefab != null)
        {
            for (int i = 0; i < currentHearts; i++)
            {
                GameObject heart = Instantiate(heartPrefab, heartsContainer);
                heartUIList.Add(heart);
            }
        }
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    private void GameOver()
    {
        isGameActive = false;

        // 禁用狗的移动
        if (dogController != null)
            dogController.DisableMovement();

        // 停止生成飞盘
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        // 显示游戏结束面板
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Debug.Log("游戏结束！2秒后自动返回主界面...");

        // 2秒后自动返回主界面
        StartCoroutine(AutoReturnToMain());
    }

    /// <summary>
    /// 自动返回主界面的协程
    /// </summary>
    private IEnumerator AutoReturnToMain()
    {
        yield return new WaitForSeconds(2f);
        BackToPark();
    }

    /// <summary>
    /// 移除已飞出屏幕的飞盘
    /// </summary>
    private void RemoveOffscreenFrisbees()
    {
        Frisbee[] allFrisbees = FindObjectsOfType<Frisbee>();
        foreach (Frisbee frisbee in allFrisbees)
        {
            if (frisbee != null && frisbee.transform.position.x < -5f) // 飞出左边界
            {
                if (!frisbee.IsCaught)
                {
                    OnFrisbeeMissed();
                }
                Destroy(frisbee.gameObject);
            }
        }
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void RestartGame()
    {
        InitializeGame();
    }

    /// <summary>
    /// 完全重置游戏状态
    /// </summary>
    private void ResetGameState()
    {
        // 重置所有游戏变量
        currentHearts = maxHearts;
        currentSpawnInterval = frisbeeSpawnInterval;
        currentFrisbeeSpeed = frisbeeSpeed;
        isGameActive = false;

        // 停止协程
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        // 清理所有飞盘
        Frisbee[] allFrisbees = FindObjectsOfType<Frisbee>();
        foreach (Frisbee frisbee in allFrisbees)
        {
            Destroy(frisbee.gameObject);
        }

        // 清理心脏UI
        foreach (GameObject heart in heartUIList)
        {
            Destroy(heart);
        }
        heartUIList.Clear();

        // 隐藏游戏结束面板
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Debug.Log("游戏状态已完全重置！");
    }

    /// <summary>
    /// 返回到ParkScene主界面
    /// </summary>
    public void BackToPark()
    {
        // 完全重置游戏状态
        ResetGameState();

        // 禁用狗的移动
        if (dogController != null)
            dogController.DisableMovement();

        // 切换界面
        if (gameCanvas != null)
            gameCanvas.SetActive(false);
        if (mainCanvas != null)
            mainCanvas.SetActive(true);

        Debug.Log("已返回主界面，游戏状态已完全重置！");
    }
}

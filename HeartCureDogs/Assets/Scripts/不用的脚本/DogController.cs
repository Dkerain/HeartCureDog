using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DogController : MonoBehaviour
{
    // 单例实例
    private static DogController _instance;
    public static DogController Instance => _instance;

    [Header("小狗预制体设置")]
    public GameObject[] dogPrefabs; // 小狗预制体数组

    [Header("全局设置")]
    public bool dontDestroyOnLoad = true;

    // 小狗实例引用
    private GameObject _dogInstance;
    private DogMovement _dogMovement;

    // 小狗标签和出生点标签
    private const string DOG_TAG = "PlayerDog";
    private const string SPAWN_POINT_TAG = "DogSpawnPoint";

    // 场景出生点存储（可选，如果你有预设的出生点位置）
    private Dictionary<string, Vector3> sceneSpawnPoints = new Dictionary<string, Vector3>();

    // 属性访问器
    public GameObject DogInstance => _dogInstance;
    public DogMovement DogMovement => _dogMovement;

    // 小狗是否已创建
    private static bool _dogCreated = false;

    private void Awake()
    {
        // 单例模式初始化
        if (_instance != null && _instance != this)
        {
            Debug.Log($"销毁重复的DogController: {gameObject.name}");
            Destroy(gameObject);
            return;
        }

        _instance = this;

        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }

        // 监听场景加载事件
        SceneManager.sceneLoaded += OnSceneLoaded;

        Debug.Log("DogController单例初始化完成");

        // 如果小狗已经存在，不再重复创建
        if (_dogCreated && _dogInstance != null)
        {
            Debug.Log("小狗已存在，跳过创建");
            return;
        }
    }

    private void Start()
    {
        // 如果小狗还没有创建，则创建小狗
        if (!_dogCreated)
        {
            CreateDog();
        }
        else
        {
            // 如果小狗已存在，找到它并更新引用
            FindExistingDog();
        }
    }

    // 创建小狗实例
    private void CreateDog()
    {
        if (dogPrefabs == null || dogPrefabs.Length == 0)
        {
            Debug.LogError("没有设置小狗预制体！");
            return;
        }

        int selectedBreed = PlayerPrefs.GetInt("SelectedBreed", 0);

        if (selectedBreed >= 0 && selectedBreed < dogPrefabs.Length)
        {
            // 获取当前场景的出生点位置
            Vector3 spawnPosition = GetCurrentSceneSpawnPoint();

            // 创建小狗实例
            _dogInstance = Instantiate(dogPrefabs[selectedBreed], spawnPosition, Quaternion.identity);

            // 设置标签
            _dogInstance.tag = DOG_TAG;

            // 获取或添加DogMovement组件
            _dogMovement = _dogInstance.GetComponent<DogMovement>();
            if (_dogMovement == null)
            {
                _dogMovement = _dogInstance.AddComponent<DogMovement>();
                Debug.Log("已添加DogMovement组件到小狗");
            }

            // 设置父物体（如果有毛毯）
            SetDogParent();

            // 让小狗实例也不随场景切换销毁
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(_dogInstance);
            }

            _dogCreated = true;

            Debug.Log($"小狗创建完成: {_dogInstance.name}");
            Debug.Log($"出生位置: {spawnPosition}");
            Debug.Log($"当前场景: {SceneManager.GetActiveScene().name}");

            // 根据场景决定是否启用移动
            UpdateMovementBasedOnScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.LogError($"小狗预制体索引超出范围！索引: {selectedBreed}, 数组长度: {dogPrefabs.Length}");
        }
    }

    // 查找已存在的小狗
    private void FindExistingDog()
    {
        GameObject existingDog = GameObject.FindGameObjectWithTag(DOG_TAG);
        if (existingDog != null)
        {
            _dogInstance = existingDog;
            _dogMovement = _dogInstance.GetComponent<DogMovement>();

            Debug.Log($"找到已存在的小狗: {_dogInstance.name}");

            // 更新位置和父物体
            UpdateDogPositionAndParent();

            // 根据场景更新移动状态
            UpdateMovementBasedOnScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.LogWarning("未找到已存在的小狗，重新创建");
            CreateDog();
        }
    }

    // 场景加载完成时的回调
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"场景已加载: {scene.name}");

        // 等待一帧，确保场景完全初始化
        StartCoroutine(ProcessAfterSceneLoad(scene.name));
    }

    private IEnumerator ProcessAfterSceneLoad(string sceneName)
    {
        yield return new WaitForEndOfFrame();

        // 确保小狗存在
        if (_dogInstance == null)
        {
            FindExistingDog();
        }

        // 更新小狗位置到新场景的出生点
        UpdateDogPositionAndParent();

        // 根据场景更新移动状态
        UpdateMovementBasedOnScene(sceneName);

        // 输出调试信息
        if (_dogInstance != null)
        {
            Debug.Log($"小狗在场景 {sceneName} 的出生点: {_dogInstance.transform.position}");
        }
    }

    // 获取当前场景的出生点位置
    private Vector3 GetCurrentSceneSpawnPoint()
    {
        // 方法1：首先查找场景中标记为DogSpawnPoint的对象
        GameObject spawnPointObj = GameObject.FindGameObjectWithTag(SPAWN_POINT_TAG);
        if (spawnPointObj != null)
        {
            Debug.Log($"找到出生点标记: {spawnPointObj.name}");
            return spawnPointObj.transform.position;
        }

        // 方法2：查找场景中是否有默认的出生点对象
        spawnPointObj = GameObject.Find("DogSpawnPoint");
        if (spawnPointObj != null)
        {
            return spawnPointObj.transform.position;
        }

        // 方法3：使用预设的出生点位置（如果有）
        if (sceneSpawnPoints.ContainsKey(SceneManager.GetActiveScene().name))
        {
            return sceneSpawnPoints[SceneManager.GetActiveScene().name];
        }

        // 方法4：使用默认位置
        Debug.LogWarning($"未找到出生点，使用默认位置 (0,0,0)");
        return Vector3.zero;
    }

    // 更新小狗位置和父物体
    private void UpdateDogPositionAndParent()
    {
        if (_dogInstance == null) return;

        // 获取新场景的出生点
        Vector3 newPosition = GetCurrentSceneSpawnPoint();
        _dogInstance.transform.position = newPosition;

        // 设置父物体
        SetDogParent();

        Debug.Log($"小狗位置已更新到: {newPosition}");
    }

    // 设置小狗的父物体
    private void SetDogParent()
    {
        if (_dogInstance == null) return;

        // 查找毛毯对象
        GameObject blanketObj = GameObject.Find("Dog");
        if (blanketObj != null)
        {
            _dogInstance.transform.SetParent(blanketObj.transform);
            Debug.Log($"小狗父物体设置为: {blanketObj.name}");
        }
        else
        {
            // 如果没有毛毯，设置为场景根节点
            _dogInstance.transform.SetParent(null);
        }
    }

    // 根据场景名称更新移动状态
    private void UpdateMovementBasedOnScene(string sceneName)
    {
        if (_dogMovement == null)
        {
            if (_dogInstance != null)
            {
                _dogMovement = _dogInstance.GetComponent<DogMovement>();
            }

            if (_dogMovement == null)
            {
                Debug.LogError("未找到DogMovement组件！");
                return;
            }
        }

        // 判断哪些场景可以移动
        // 这里可以根据你的场景名称来调整规则
        if (sceneName.Contains("Home") || sceneName.Contains("House") ||
            sceneName.Contains("Indoor") || sceneName.Contains("室内"))
        {
            DisableDogMovement();
        }
        else if (sceneName.Contains("Park") || sceneName.Contains("Street") ||
                 sceneName.Contains("Outdoor") || sceneName.Contains("户外"))
        {
            EnableDogMovement();
        }
        else
        {
            // 默认可以移动
            EnableDogMovement();
        }
    }

    // 公共方法 - 启用小狗移动
    public void EnableDogMovement()
    {
        if (_dogMovement != null)
        {
            _dogMovement.EnableFreeMovement();
            Debug.Log("小狗移动已启用");
        }
        else if (_dogInstance != null)
        {
            Debug.LogWarning("DogMovement组件未找到，尝试获取...");
            _dogMovement = _dogInstance.GetComponent<DogMovement>();

            if (_dogMovement != null)
            {
                _dogMovement.EnableFreeMovement();
            }
        }
    }

    // 公共方法 - 禁用小狗移动
    public void DisableDogMovement()
    {
        if (_dogMovement != null)
        {
            _dogMovement.DisableMovement();
            Debug.Log("小狗移动已禁用");
        }
    }

    // 设置特定场景的出生点（可以通过其他脚本调用）
    public void SetSceneSpawnPoint(string sceneName, Vector3 spawnPoint)
    {
        if (sceneSpawnPoints.ContainsKey(sceneName))
        {
            sceneSpawnPoints[sceneName] = spawnPoint;
        }
        else
        {
            sceneSpawnPoints.Add(sceneName, spawnPoint);
        }

        Debug.Log($"已设置场景 {sceneName} 的出生点: {spawnPoint}");
    }

    // 移动小狗到指定位置
    public void MoveDogToPosition(Vector3 position)
    {
        if (_dogInstance != null)
        {
            _dogInstance.transform.position = position;
            Debug.Log($"强制移动小狗到: {position}");
        }
    }

    // 销毁小狗实例（用于重新创建）
    public void DestroyDog()
    {
        if (_dogInstance != null)
        {
            Destroy(_dogInstance);
            _dogInstance = null;
            _dogMovement = null;
            _dogCreated = false;
            Debug.Log("小狗实例已销毁");
        }
    }

    // 重新创建小狗（例如更换品种后）
    public void RecreateDog()
    {
        DestroyDog();
        CreateDog();
    }

    private void OnDestroy()
    {
        // 取消订阅事件
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // 重置静态变量
        if (_instance == this)
        {
            _instance = null;
            _dogCreated = false;
        }
    }


}
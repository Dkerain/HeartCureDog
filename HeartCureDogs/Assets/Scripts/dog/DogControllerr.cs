using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogControllerr : MonoBehaviour
{
    [Header("动画控制")]
    public Animator dogAnimator;
    [Header("移动参数")]
    [Tooltip("调整小狗移动速度")]
    public float moveSpeed = 5f; // 默认速度，面板可调
    [Header("移动限制")]
    public LayerMask walkableLayer;
    private Vector3 targetPosition; // 存储目标位置
    private bool isMoving = false; // 移动状态标记
    [Header("实例化设置")]
    public GameObject[] dogPrefabs; // 添加这行
    public Transform spawnPoint;    // 添加这行
    public Transform blanket; // 新增毛毯引用

    void Start()
    {
        SpawnDog(); // 新增生成方法
        // 初始位置作为第一个目标点
        targetPosition = transform.position;
    }
    void SpawnDog()
    {
        int selectedBreed = PlayerPrefs.GetInt("SelectedBreed", 0);
        if (selectedBreed >= 0 && dogPrefabs.Length > selectedBreed)
        {
            GameObject newDog = Instantiate(dogPrefabs[selectedBreed], spawnPoint.position, Quaternion.identity);
            dogAnimator = newDog.GetComponent<Animator>();
            newDog.transform.SetParent(blanket);
        }
    }
    public void EnableControl() => enabled = true;
    void Update()
    {
        // 仅在左键点击时更新目标点
        if (Input.GetMouseButtonDown(0))
        {
            SetTargetPosition();
        }

        // 持续向目标点移动
        MoveToTarget();
        UpdateSpriteRotation(); // 新增此行
        dogAnimator.SetBool("IsWalking", isMoving);
    }

    void SetTargetPosition()
    {
        Vector3 worldPos = GetMouseWorldPos();

        // 检测目标点是否在可移动区域
        if (IsPositionWalkable(worldPos))
        {
            targetPosition = worldPos;
            isMoving = true;
        }
    }
    bool IsPositionWalkable(Vector3 pos)
    {
        Collider2D hit = Physics2D.OverlapPoint(pos, walkableLayer);
        return hit != null;
    }

    void MoveToTarget()
    {
        if (!isMoving) return;

        // 计算移动步长（时间相关）
        float step = moveSpeed * Time.deltaTime;

        // 移动并转向目标
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            step
        );

        // 到达目标点停止
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            isMoving = false;
        }
    }
    void UpdateSpriteRotation()
    {
        // 根据移动方向水平翻转精灵
        if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(-0.2016079f, 0.2016079f, 1); // 朝右
        }
        else if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(0.2016079f, 0.2016079f, 1); // 朝左
        }
        float moveDir = targetPosition.x - transform.position.x;
        dogAnimator.SetFloat("MoveX", moveDir); // X轴移动方向
    }
    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    public void SetTargetPosition(Vector3 target)
    {
        if (IsPositionWalkable(target))
        {
            targetPosition = target;
            isMoving = true;
        }
    }

}

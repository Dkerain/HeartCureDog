using System.Collections;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    [Header("移动参数")]
    [SerializeField] private float moveSpeed = 3f;     // Serialized调参更安全
    [SerializeField] private LayerMask groundLayer;    // 暴露给编辑器设置
    [Range(0.1f, 1f)] public float stopDistance = 0.1f;

    [Header("时间缩放修复")]
    [SerializeField] private bool useUnscaledTime = true; // 使用不受时间缩放影响的时间

    [Header("调试模式")]
    public bool showDebug = true;

    // 状态控制
    private Vector3? pendingTarget = null;
    private bool canMove = false;
    private bool isMoving = false;

    // 组件缓存
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main; // 缓存主摄像机
        if (mainCamera == null) Debug.LogError("场景中缺少主摄像机");

        // 自动设置地面层（如果未设置）
        if (groundLayer.value == 0)
        {
            groundLayer = LayerMask.GetMask("Ground", "Default");
            if (showDebug) Debug.Log("自动设置地面层");
        }
    }

    void Update()
    {
        if (!canMove || isMoving) return;

        // 修复1：使用 MouseDown 代替 Mouse
        if (Input.GetMouseButtonDown(0))
        {
            // 可选：检查是否点击在UI上
            if (IsPointerOverUI()) return;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                if (showDebug)
                {
                    Debug.Log($"有效点击坐标: {hit.point}");
                    Debug.DrawLine(transform.position, hit.point, Color.red, 1f);
                }

                if (isMoving)
                {
                    pendingTarget = hit.point; // 排队新目标
                }
                else
                {
                    StartCoroutine(SmoothMove(hit.point));
                }
            }
        }
    }

    private IEnumerator SmoothMove(Vector3 target)
    {
        isMoving = true;
        target.z = transform.position.z; // 修复2：保持Y轴一致（按需修改）

        float journeyLength = Vector3.Distance(transform.position, target);
        if (journeyLength < stopDistance)
        {
            isMoving = false;
            yield break;
        }

        // 使用基于速度的移动计算
        // 根据设置选择使用Time.time还是Time.unscaledTime
        float startTime = useUnscaledTime ? Time.unscaledTime : Time.time;
        Vector3 startPosition = transform.position;

        while (Vector3.Distance(transform.position, target) > stopDistance)
        {
            // 优先级处理新目标
            if (pendingTarget.HasValue)
            {
                startPosition = transform.position;
                target = pendingTarget.Value;
                journeyLength = Vector3.Distance(startPosition, target);
                startTime = useUnscaledTime ? Time.unscaledTime : Time.time;
                pendingTarget = null;
                if (showDebug) Debug.Log($"更新目标至：{target}");
            }

            // 根据设置选择时间源
            float currentTime = useUnscaledTime ? Time.unscaledTime : Time.time;
            float distanceCovered = (currentTime - startTime) * moveSpeed;
            float progress = distanceCovered / journeyLength;

            // 添加平滑曲线：前慢-快-后慢
            progress = Mathf.SmoothStep(0f, 1f, progress);

            transform.position = Vector3.Lerp(startPosition, target, progress);
            yield return null;
        }

        transform.position = target;
        isMoving = false;
        if (showDebug) Debug.Log("移动完成");
    }

    public void EnableFreeMovement()
    {
        canMove = true;
        pendingTarget = null;

        // 确保主摄像机已缓存
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (showDebug) Debug.Log("移动系统已激活");
    }

    public void DisableMovement()
    {
        canMove = false;
        StopAllCoroutines();
        isMoving = false; // 重置移动状态
        pendingTarget = null;
        if (showDebug) Debug.Log("移动系统已禁用");
    }

    // 新增：检查是否可以移动
    public bool CanMove()
    {
        return canMove;
    }

    // 新增：检查是否正在移动
    public bool IsMoving()
    {
        return isMoving;
    }

    // 新增：重置移动状态
    public void ResetMovement()
    {
        canMove = false;
        isMoving = false;
        pendingTarget = null;
        StopAllCoroutines();
        if (showDebug) Debug.Log("移动系统已重置");
    }

    // 新增：检查鼠标是否点击在UI上
    private bool IsPointerOverUI()
    {
        // 如果有EventSystem且鼠标在UI上，返回true
        if (UnityEngine.EventSystems.EventSystem.current != null &&
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }

    // 新增：设置移动速度
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = Mathf.Max(0.1f, newSpeed); // 确保速度不小于0.1
        if (showDebug) Debug.Log($"移动速度设置为: {moveSpeed}");
    }

    // 新增：获取当前移动目标（用于调试）
    public Vector3? GetCurrentTarget()
    {
        return pendingTarget;
    }
}
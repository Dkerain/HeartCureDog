using System.Collections;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    [Header("移动参数")]
    [SerializeField] private float moveSpeed = 3f;     // Serialized调参更安全
    [SerializeField] private LayerMask groundLayer;    // 暴露给编辑器设置
    [Range(0.1f, 1f)] public float stopDistance = 0.1f;

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
    }

    void Update()
    {
        if (!canMove || isMoving) return;

        // 修复1：使用 MouseDown 代替 Mouse
        if (Input.GetMouseButtonDown(0))
        {
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
        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        while (Vector3.Distance(transform.position, target) > stopDistance)
        {
            // 优先级处理新目标
            if (pendingTarget.HasValue)
            {
                startPosition = transform.position;
                target = pendingTarget.Value;
                journeyLength = Vector3.Distance(startPosition, target);
                startTime = Time.time;
                pendingTarget = null;
                if (showDebug) Debug.Log($"更新目标至：{target}");
            }

            float distanceCovered = (Time.time - startTime) * moveSpeed;
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
        if (showDebug) Debug.Log("移动系统已激活");
    }

    public void DisableMovement()
    {
        canMove = false;
        StopAllCoroutines();
        if (showDebug) Debug.Log("移动系统已禁用");
    }
}

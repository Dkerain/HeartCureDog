using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogControllerr : MonoBehaviour
{
    [Header("移动参数")]
    [Tooltip("调整小狗移动速度")]
    public float moveSpeed = 5f; // 默认速度，面板可调

    private Vector3 targetPosition; // 存储目标位置
    private bool isMoving = false; // 移动状态标记

    void Start()
    {
        // 初始位置作为第一个目标点
        targetPosition = transform.position;
    }

    void Update()
    {
        // 仅在左键点击时更新目标点
        if (Input.GetMouseButtonDown(0))
        {
            SetTargetPosition();
        }

        // 持续向目标点移动
        MoveToTarget();
    }

    void SetTargetPosition()
    {
        // 将鼠标屏幕坐标转换为世界坐标
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // 确保z值与相机距离一致

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        targetPosition = new Vector3(worldPos.x, worldPos.y, 0);

        isMoving = true; // 进入移动状态
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
}

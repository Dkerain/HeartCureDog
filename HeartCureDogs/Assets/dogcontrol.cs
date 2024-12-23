using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 20.0f; // 角色移动速度

    private Rigidbody2D rb;
    private Camera mainCamera; // 引用主相机

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main; // 获取主相机的引用
    }

    void Update()
    {
        // 获取WASD键的输入
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // 创建移动向量
        Vector2 movement = new Vector2(moveX, moveY);

        // 应用移动
        if (movement != Vector2.zero)
        {
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        }
    }

    void LateUpdate()
    {
        // 获取屏幕边界
        float halfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float halfHeight = mainCamera.orthographicSize;

        // 将世界坐标转换为视口坐标
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(rb.position);

        // 确保角色在屏幕边界内
        if (viewportPosition.x < 0.0f || viewportPosition.x > 1.0f || viewportPosition.y < 0.0f || viewportPosition.y > 1.0f)
        {
            // 将角色移回屏幕内
            Vector3 clampedViewportPosition = new Vector3(
                Mathf.Clamp(viewportPosition.x, 0.0f, 1.0f),
                Mathf.Clamp(viewportPosition.y, 0.0f, 1.0f),
                viewportPosition.z
            );
            Vector3 clampedWorldPosition = mainCamera.ViewportToWorldPoint(clampedViewportPosition);
            rb.position = clampedWorldPosition;
            rb.velocity = Vector2.zero; // 当角色在边界时，停止移动
        }
    }
}
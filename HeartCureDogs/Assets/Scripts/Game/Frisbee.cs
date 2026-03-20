using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 飞盘脚本：控制飞盘的飞行和状态
/// </summary>
public class Frisbee : MonoBehaviour
{
    [Tooltip("飞盘移动速度")]
    private float moveSpeed = 5f;

    [Tooltip("飞盘是否被接住")]
    public bool IsCaught { get; private set; } = false;

    void Start()
    {
        // 初始化，不需要特殊设置
    }

    void Update()
    {
        if (!IsCaught)
        {
            MoveFrisbee();
        }
    }

    /// <summary>
    /// 移动飞盘（从右向左）
    /// </summary>
    private void MoveFrisbee()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 设置飞盘速度
    /// </summary>
    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    /// <summary>
    /// 飞盘被接住时的处理
    /// </summary>
    public void Catch()
    {
        IsCaught = true;
        // 可以添加音效、动画等效果
        Debug.Log("飞盘被接住了！");
        // 延迟销毁，以便显示动画
        Destroy(gameObject, 0.2f);
    }
}

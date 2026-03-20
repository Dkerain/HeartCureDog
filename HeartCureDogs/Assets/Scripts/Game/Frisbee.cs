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

    private FrisbeeGame gameManager;

    void Start()
    {
        // 初始化，尝试获取游戏管理器
        gameManager = FindObjectOfType<FrisbeeGame>();
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

    /// <summary>
    /// 设置游戏管理器引用
    /// </summary>
    public void SetGameManager(FrisbeeGame manager)
    {
        gameManager = manager;
    }

    /// <summary>
    /// 碰撞检测 - 当飞盘与小狗碰撞时触发
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsCaught) return;

        // 检测是否与小狗相撞（通过DogControllerr脚本判断）
        DogControllerr dogController = collision.GetComponent<DogControllerr>();
        if (dogController != null)
        {
            Debug.Log("飞盘与狗发生碰撞！接住了！");
            if (gameManager != null)
            {
                gameManager.OnFrisbeeCaught();
            }
            Catch();
        }
    }
}

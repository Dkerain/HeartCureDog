using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.DialogueTrees;
using UnityEngine.UI; // 添加命名空间引用
public class DogControllerr : MonoBehaviour
{
    public bool IsMoving => isMoving;   // 只读属性，返回 isMoving 的值
    [Header("动画控制")]
    public Animator dogAnimator;
    [Header("移动参数")]
    [Tooltip("调整小狗移动速度")]
    public float moveSpeed = 5f; // 默认速度，面板可调
    [Header("移动限制")]
    public LayerMask walkableLayer;
    private Vector3 targetPosition; // 存储目标位置
    private bool isMoving = false; // 移动状态标记
    [Header("生成设置")]

    [Header("触发区域设置")]
    public GameObject button; // 要触发的button
    public string squareTag = "TriggerSquare"; // 触发区域的标签

    private GameObject currentDog;
    private bool canMove = false;
    private bool isInTriggerSquare = false; // 是否在触发区域内
    void Start()
    {
       
        DisableMovement(); // 初始时禁用移动
        // 初始位置作为第一个目标点
        targetPosition = transform.position;

        // 确保Panel初始状态为关闭
        if (button != null)
        {
            button.SetActive(false);
        }
    }

    void Update()
    {
        if (!canMove) return;
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
            CheckTriggerSquare();
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

    // 碰撞检测方法
    void OnTriggerEnter2D(Collider2D other)
    {
        // 检查是否进入触发区域
        if (other.CompareTag(squareTag))
        {
            isInTriggerSquare = true;
            Showbutton();
            Debug.Log("进入触发区域");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // 检查是否离开触发区域
        if (other.CompareTag(squareTag))
        {
            isInTriggerSquare = false;
            Hidebutton();
            Debug.Log("离开触发区域");
        }
    }

    // 显示button的方法
    void Showbutton()
    {
        if (button != null && !button.activeSelf)
        {
            button.SetActive(true);
            Debug.Log("Panel已显示");
        }
    }

    // 隐藏button的方法
    void Hidebutton()
    {
        if (button != null && button.activeSelf)
        {
            button.SetActive(false);
            Debug.Log("button已隐藏");
        }
    }
    // 检查并触发button
    void CheckTriggerSquare()
    {
        /*if (isInTriggerSquare && button != null)
        {
            button.SetActive(true);
            Debug.Log("触发button显示");

            // 可选：触发button时暂停移动
            // DisableMovement();
        }
        else if (button != null)
        {
            button.SetActive(false);
        }*/
        Debug.Log($"当前触发状态: isInTriggerSquare = {isInTriggerSquare}, Button状态: {(button != null ? button.activeSelf : false)}");
    }

    void OnEnable()
    {
        DialogueTree.OnDialogueStarted += OnDialogueStarted;
        DialogueTree.OnDialogueFinished += OnDialogueFinished;
    }

    void OnDisable()
    {
        DialogueTree.OnDialogueStarted -= OnDialogueStarted;
        DialogueTree.OnDialogueFinished -= OnDialogueFinished;
    }

    private void OnDialogueStarted(DialogueTree dlg)
    {
        DisableMovement();
    }

    private void OnDialogueFinished(DialogueTree dlg)
    {
        EnableMovement();
    }



    public void EnableMovement() => canMove = true;
    public void DisableMovement() => canMove = false;
}
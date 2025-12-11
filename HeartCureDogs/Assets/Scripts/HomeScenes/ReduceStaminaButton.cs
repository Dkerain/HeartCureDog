using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodeCanvas.Framework;

public class ReduceStaminaButton : MonoBehaviour
{
    private Button reduceButton;
    [SerializeField] private int staminaReduceAmount = 5; // 每次减少的体力值

    void Start()
    {
        reduceButton = GetComponent<Button>();
        if (reduceButton != null)
        {
            reduceButton.onClick.AddListener(OnReduceButtonClick);
        }
    }

    void OnReduceButtonClick()
    {
        Debug.Log("减少体力按钮被点击");

        // 获取全局黑板
        GlobalBlackboard gb = GlobalBlackboard.Find("Global");
        if (gb != null)
        {
            // 检查npcBrwanValue变量是否存在
            Variable variable = gb.GetVariable("npcBrwanValue");
            if (variable != null)
            {
                // 获取当前体力值
                int currentStamina = gb.GetVariableValue<int>("npcBrwanValue");

                // 减少体力
                int newStamina = currentStamina - staminaReduceAmount;

                // 防止体力变为负数（可选）
                if (newStamina < 0)
                {
                    newStamina = 0;
                }

                // 设置新的体力值
                gb.SetVariableValue("npcBrwanValue", newStamina);

                Debug.Log($"体力减少: {staminaReduceAmount}点 | 当前体力: {newStamina}");

                // 可选：如果体力为0，可以触发其他事件
                if (newStamina <= 0)
                {
                    Debug.Log("体力已耗尽！");
                    // 可以在这里触发其他逻辑，比如小狗需要休息
                    // 例如：gb.SetVariableValue("isExhausted", true);
                }
            }
            else
            {
                Debug.LogError("全局黑板中没有找到 'npcBrwanValue' 变量！");
            }
        }
        else
        {
            Debug.LogError("未找到名为 'Global' 的全局黑板！");
        }
    }

    // 可选：添加一个公共方法，允许从其他脚本触发体力减少
    public void ReduceStamina()
    {
        OnReduceButtonClick();
    }

    // 可选：添加一个方法允许动态设置减少值
    public void SetReduceAmount(int amount)
    {
        staminaReduceAmount = Mathf.Max(0, amount); // 确保不为负数
    }

    // 可选：添加一个方法检查是否还有体力可以消耗
    public bool CanReduceStamina()
    {
        GlobalBlackboard gb = GlobalBlackboard.Find("Global");
        if (gb != null)
        {
            Variable variable = gb.GetVariable("npcBrwanValue");
            if (variable != null)
            {
                int currentStamina = gb.GetVariableValue<int>("npcBrwanValue");
                return currentStamina >= staminaReduceAmount;
            }
        }
        return false;
    }

    // 可选：在体力不足时自动禁用按钮
    void Update()
    {
        if (reduceButton != null)
        {
            // 根据体力是否足够来启用/禁用按钮
            reduceButton.interactable = CanReduceStamina();
        }
    }

    void OnDestroy()
    {
        if (reduceButton != null)
        {
            reduceButton.onClick.RemoveListener(OnReduceButtonClick);
        }
    }
}
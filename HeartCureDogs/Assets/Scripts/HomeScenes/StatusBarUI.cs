using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NodeCanvas.Framework;

public class StatusBarUI : MonoBehaviour
{
    [Header("UI文本引用")]
    public TextMeshProUGUI playerEnergyText;  // 玩家体力值
    public TextMeshProUGUI playerCoinsText;   // 玩家金币
    public TextMeshProUGUI dogHealthText;     // 狗狗健康值
    public TextMeshProUGUI dogEnergyText;     // 狗狗精力值
    public TextMeshProUGUI dogTrustText;      // 狗狗信任值
    public TextMeshProUGUI dogMoodText;       // 狗狗情绪

    private GlobalBlackboard globalBlackboard;
    private bool blackboardFound = false;

    private void Start()
    {
        Debug.Log("UI脚本启动");
        FindGlobalBlackboard();
        UpdateUI();
    }

    private void FindGlobalBlackboard()
    {
        globalBlackboard = GlobalBlackboard.Find("Global");

        if (globalBlackboard != null)
        {
            blackboardFound = true;
            Debug.Log("UI成功找到全局黑板！");

            // 测试读取值
            try
            {
                Debug.Log($"测试读取 - 金币: {globalBlackboard.GetValue<int>("npcCoinValue")}");
                Debug.Log($"测试读取 - 体力: {globalBlackboard.GetValue<int>("npcBrwanValue")}");
                Debug.Log($"测试读取 - 精力: {globalBlackboard.GetValue<int>("dogEnergyValue")}");
                Debug.Log($"测试读取 - 健康: {globalBlackboard.GetValue<int>("dogHealthValue")}");
                Debug.Log($"测试读取 - 信任: {globalBlackboard.GetValue<int>("dogBelieveValue")}");
                Debug.Log($"测试读取 - 情绪: {globalBlackboard.GetValue<string>("dogEmotion")}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"读取全局黑板值时出错: {e.Message}");
            }
        }
        else
        {
            blackboardFound = false;
            Debug.LogError("UI未找到全局黑板！请确保场景中有标识为'Global'的GlobalBlackboard对象");
        }
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (!blackboardFound || globalBlackboard == null)
        {
            DisplayDefaultValues();
            return;
        }

        try
        {
            // 从全局黑板读取值并更新UI
            if (playerEnergyText != null)
            {
                int value = globalBlackboard.GetValue<int>("npcBrwanValue");
                playerEnergyText.text = "体力值: " + value;
                Debug.Log($"更新玩家体力值UI: {value}");
            }

            if (playerCoinsText != null)
            {
                int value = globalBlackboard.GetValue<int>("npcCoinValue");
                playerCoinsText.text = "金币: " + value;
                Debug.Log($"更新金币UI: {value}");
            }

            if (dogHealthText != null)
            {
                int value = globalBlackboard.GetValue<int>("dogHealthValue");
                dogHealthText.text = "健康值: " + value;
                Debug.Log($"更新健康值UI: {value}");
            }

            if (dogEnergyText != null)
            {
                int value = globalBlackboard.GetValue<int>("dogEnergyValue");
                dogEnergyText.text = "精力值: " + value;
                Debug.Log($"更新精力值UI: {value}");
            }

            if (dogTrustText != null)
            {
                int value = globalBlackboard.GetValue<int>("dogBelieveValue");
                dogTrustText.text = "信任值: " + value;
                Debug.Log($"更新信任值UI: {value}");
            }

            if (dogMoodText != null)
            {
                string value = globalBlackboard.GetValue<string>("dogEmotion");
                dogMoodText.text = "情绪: " + value;
                Debug.Log($"更新情绪UI: {value}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("更新UI时出错: " + e.Message);
            DisplayDefaultValues();
        }
    }

    private void DisplayDefaultValues()
    {
        Debug.Log("显示默认值");

        // 找不到黑板时显示默认值
        if (playerEnergyText != null)
            playerEnergyText.text = "体力值: 45";

        if (playerCoinsText != null)
            playerCoinsText.text = "金币: 50";

        if (dogHealthText != null)
            dogHealthText.text = "健康值: 50";

        if (dogEnergyText != null)
            dogEnergyText.text = "精力值: 50";

        if (dogTrustText != null)
            dogTrustText.text = "信任值: 50";

        if (dogMoodText != null)
            dogMoodText.text = "情绪: 平静";
    }

    public void RefreshUI()
    {
        Debug.Log("手动刷新UI");
        UpdateUI();
    }

    private void OnEnable()
    {
        Debug.Log("UI组件启用");
        UpdateUI();
    }
}
/*using UnityEngine;
using TMPro;
using NodeCanvas.Framework;

public class StatusBarUI : MonoBehaviour
{
    public TextMeshProUGUI playerEnergyText;
    public TextMeshProUGUI playerCoinsText;
    public TextMeshProUGUI dogHealthText;
    public TextMeshProUGUI dogEnergyText;
    public TextMeshProUGUI dogTrustText;
    public TextMeshProUGUI dogMoodText;

    private Blackboard globalBlackboard;

    private void Start()
    {
        globalBlackboard = GlobalBlackboard.Find("Global");

        if (globalBlackboard == null)
        {
            Debug.LogError("δ�ҵ�ȫ�ֺڰ壡");
            return;
        }

        // ��ʼ����
        UpdateAllUI();
    }

    private void Update()
    {
        // ÿ֡���£��򵥵�Ч�ʽϵͣ�
        UpdateAllUI();
    }

    private void UpdateAllUI()
    {
        if (globalBlackboard == null) return;

        try
        {
            if (playerEnergyText != null)
                playerEnergyText.text = "����ֵ: " + globalBlackboard.GetVariableValue<int>("npcBrwanValue");
            if (playerCoinsText != null)
                playerCoinsText.text = "���: " + globalBlackboard.GetVariableValue<int>("npcCoinValue");
            if (dogHealthText != null)
                dogHealthText.text = "����ֵ: " + globalBlackboard.GetVariableValue<int>("dogHealthValue");
            if (dogEnergyText != null)
                dogEnergyText.text = "����ֵ: " + globalBlackboard.GetVariableValue<int>("dogEnergyValue");
            if (dogTrustText != null)
                dogTrustText.text = "����ֵ: " + globalBlackboard.GetVariableValue<int>("dogBelieveValue");
            if (dogMoodText != null)
                dogMoodText.text = "����: " + globalBlackboard.GetVariableValue<string>("dogEmotion");
        }
        catch (System.Exception e)
        {
            Debug.LogError("����UIʱ����: " + e.Message);
        }
    }
}*/
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusBarUI : MonoBehaviour
{
    public TextMeshProUGUI playerEnergyText;
    public TextMeshProUGUI playerCoinsText;
    public TextMeshProUGUI dogHealthText;
    public TextMeshProUGUI dogEnergyText;
    public TextMeshProUGUI dogTrustText;
    public TextMeshProUGUI dogMoodText;

    private void Start()
    {
        //UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (playerEnergyText != null)
            playerEnergyText.text = "体力值: " + PlayerStatus.Instance.playerEnergy;
        if (playerCoinsText != null)
            playerCoinsText.text = "金币: " + PlayerStatus.Instance.playerCoins;
        if (dogHealthText != null)
            dogHealthText.text = "体魄值: " + PlayerStatus.Instance.dogHealth;
        if (dogEnergyText != null)
            dogEnergyText.text = "精力值: " + PlayerStatus.Instance.dogEnergy;
        if (dogTrustText != null)
            dogTrustText.text = "信任值: " + PlayerStatus.Instance.dogTrust;
        if (dogMoodText != null)
            dogMoodText.text = "心情: " + PlayerStatus.Instance.dogMood;
    }
}*/
using UnityEngine;
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
            Debug.LogError("未找到全局黑板！");
            return;
        }

        // 初始更新
        UpdateAllUI();
    }

    private void Update()
    {
        // 每帧更新（简单但效率较低）
        UpdateAllUI();
    }

    private void UpdateAllUI()
    {
        if (globalBlackboard == null) return;

        try
        {
            if (playerEnergyText != null)
                playerEnergyText.text = "体力值: " + globalBlackboard.GetVariableValue<int>("npcBrwanValue");
            if (playerCoinsText != null)
                playerCoinsText.text = "金币: " + globalBlackboard.GetVariableValue<int>("npcCoinValue");
            if (dogHealthText != null)
                dogHealthText.text = "体魄值: " + globalBlackboard.GetVariableValue<int>("dogHealthValue");
            if (dogEnergyText != null)
                dogEnergyText.text = "精力值: " + globalBlackboard.GetVariableValue<int>("dogEnergyValue");
            if (dogTrustText != null)
                dogTrustText.text = "信任值: " + globalBlackboard.GetVariableValue<int>("dogBelieveValue");
            if (dogMoodText != null)
                dogMoodText.text = "心情: " + globalBlackboard.GetVariableValue<string>("dogEmotion");
        }
        catch (System.Exception e)
        {
            Debug.LogError("更新UI时出错: " + e.Message);
        }
    }
}
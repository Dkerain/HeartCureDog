using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodeCanvas.Framework;
using TMPro;

public class cm : MonoBehaviour
{
    // 声明UI文本组件
    
    public TextMeshProUGUI dogEmotionText;
    public TextMeshProUGUI dogHealthText;
    public TextMeshProUGUI dogBelieveText;
    public TextMeshProUGUI dogEnergyText;

    
    public TextMeshProUGUI npcCoinText;
    public TextMeshProUGUI npcBrwanText;

    // 全局黑板
    public GlobalBlackboard globalBlackboard;

    void Start()
    {
        // 初始化黑板cm

        // 更新UI显示
        UpdateCharacterUI();
    }

    void InitializeBlackboard()
    {
        // 初始化小狗的属性
        if (globalBlackboard != null)
        {
            
            globalBlackboard.SetValue("dogEmotion", "平静");
            globalBlackboard.SetValue("dogHealthValue", 50);
            globalBlackboard.SetValue("dogBelieveValue", 50);
            globalBlackboard.SetValue("dogEnergyValue", 50);

            // 初始化NPC的属性
            
            globalBlackboard.SetValue("npcCoinValue", 50);
            globalBlackboard.SetValue("npcBrwanValue", 45);
        }
    }

    void UpdateCharacterUI()
    {
        // 更新小狗UI
        if (globalBlackboard != null)
        {
            
            dogEmotionText.text = $"心情: {globalBlackboard.GetValue<string>("dogEmotion")}";
            dogHealthText.text = $"健康值: {globalBlackboard.GetValue<int>("dogHealthValue")}";
            dogBelieveText.text = $"信任值: {globalBlackboard.GetValue<int>("dogBelieveValue")}";
            dogEnergyText.text = $"精力值: {globalBlackboard.GetValue<int>("dogEnergyValue")}";

            // 更新NPC UI
           
            npcCoinText.text = $"金币: {globalBlackboard.GetValue<int>("npcCoinValue")}";
            npcBrwanText.text = $"体力值: {globalBlackboard.GetValue<int>("npcBrwanValue")}";
        }
    }

    // 在对话树中更新小狗属性的方法
    public void UpdateDogStats(string emotion, int health, int believe, int energy)
    {
        if (globalBlackboard != null)
        {
            globalBlackboard.SetValue("dogEmotion", emotion);
            globalBlackboard.SetValue("dogHealthValue", health);
            globalBlackboard.SetValue("dogBelieveValue", believe);
            globalBlackboard.SetValue("dogEnergyValue", energy);
            UpdateCharacterUI();
        }
    }

    // 在对话树中更新NPC属性的方法
    public void UpdateNPCStats(int coin, int brwan)
    {
        if (globalBlackboard != null)
        {
            globalBlackboard.SetValue("npcCoinValue", coin);
            globalBlackboard.SetValue("npcBrwanValue", brwan);
            UpdateCharacterUI();
        }
    }
     // 在对话树中手动调用UI更新的方法
    public void RefreshUI()
    {
        UpdateCharacterUI();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;

public class DogAttributeManager : MonoBehaviour
{
    [Header("属性最大值")]
    public int maxDogEnergy = 100;
    public int maxDogHealth = 100;
    public int maxDogBelieve = 100;
    public int maxNpcBrwan = 100;

    [Header("初始值配置")]
    public int initialDogEnergy = 50;
    public int initialDogHealth = 50;
    public int initialDogBelieve = 50;
    public string initialDogEmotion = "平静";
    public int initialNpcCoin = 50;
    public int initialNpcBrwan = 45;

    private GlobalBlackboard globalBlackboard;

    private void Awake()
    {
        FindGlobalBlackboard();
        InitializeAttributes();
        LoadAttributes();
    }

    private void FindGlobalBlackboard()
    {
        globalBlackboard = GlobalBlackboard.Find("Global");

        if (globalBlackboard != null)
        {
            Debug.Log("成功找到全局黑板");
        }
        else
        {
            Debug.LogError("未找到全局黑板！请确保场景中有标识符为'Global'的GlobalBlackboard。");
        }
    }

    private void InitializeAttributes()
    {
        if (globalBlackboard == null) return;

        // 使用正确的方法检查变量是否存在
        InitializeVariable("dogEnergyValue", initialDogEnergy);
        InitializeVariable("dogHealthValue", initialDogHealth);
        InitializeVariable("dogBelieveValue", initialDogBelieve);
        InitializeVariable("dogEmotion", initialDogEmotion);
        InitializeVariable("npcCoinValue", initialNpcCoin);
        InitializeVariable("npcBrwanValue", initialNpcBrwan);

        Debug.Log("属性初始化完成");
    }

    private void InitializeVariable(string variableName, object defaultValue)
    {
        try
        {
            // 尝试获取变量，如果抛出异常说明变量不存在
            var variable = globalBlackboard.GetVariable(variableName);
            if (variable == null)
            {
                // 变量不存在，创建它
                globalBlackboard.SetValue(variableName, defaultValue);
            }
        }
        catch (System.Exception)
        {
            // 变量不存在，创建它
            globalBlackboard.SetValue(variableName, defaultValue);
        }
    }

    // ========== 无参数的简单方法 ==========

    public void AddDogEnergy10()
    {
        AddDogEnergy(10);
    }

    public void AddDogEnergy5()
    {
        AddDogEnergy(5);
    }

    public void AddDogEnergyMinus5()
    {
        AddDogEnergy(-5);
    }

    public void AddDogHealth10()
    {
        AddDogHealth(10);
    }

    public void AddDogHealth5()
    {
        AddDogHealth(5);
    }

    public void AddDogBelieve10()
    {
        AddDogBelieve(10);
    }

    public void AddDogBelieve5()
    {
        AddDogBelieve(5);
    }

    public void AddNpcCoin100()
    {
        AddNpcCoin(100);
    }

    public void AddNpcCoin50()
    {
        AddNpcCoin(50);
    }

    public void AddNpcBrwan10()
    {
        AddNpcBrwan(10);
    }

    public void AddNpcBrwanMinus10()
    {
        AddNpcBrwan(-10);
    }

    public void SetDogEmotionHappy()
    {
        SetDogEmotion("开心");
    }

    public void SetDogEmotionCalm()
    {
        SetDogEmotion("平静");
    }

    public void SetDogEmotionSad()
    {
        SetDogEmotion("悲伤");
    }

    // ========== 带参数的公共方法 ==========

    public void AddDogEnergy(int amount)
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return;

        int current = globalBlackboard.GetValue<int>("dogEnergyValue");
        int newValue = Mathf.Clamp(current + amount, 0, maxDogEnergy);
        globalBlackboard.SetValue("dogEnergyValue", newValue);
        SaveAttributes();
        Debug.Log($"小狗精力值变化: {current} -> {newValue}");
    }

    public void AddDogHealth(int amount)
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return;

        int current = globalBlackboard.GetValue<int>("dogHealthValue");
        int newValue = Mathf.Clamp(current + amount, 0, maxDogHealth);
        globalBlackboard.SetValue("dogHealthValue", newValue);
        SaveAttributes();
        Debug.Log($"小狗健康值变化: {current} -> {newValue}");
    }

    public void AddDogBelieve(int amount)
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return;

        int current = globalBlackboard.GetValue<int>("dogBelieveValue");
        int newValue = Mathf.Clamp(current + amount, 0, maxDogBelieve);
        globalBlackboard.SetValue("dogBelieveValue", newValue);
        SaveAttributes();
        Debug.Log($"小狗信任值变化: {current} -> {newValue}");
    }

    public void AddNpcCoin(int amount)
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return;

        int current = globalBlackboard.GetValue<int>("npcCoinValue");
        int newValue = Mathf.Max(0, current + amount);
        globalBlackboard.SetValue("npcCoinValue", newValue);
        SaveAttributes();
        Debug.Log($"玩家金币变化: {current} -> {newValue}");
    }

    public void AddNpcBrwan(int amount)
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return;

        int current = globalBlackboard.GetValue<int>("npcBrwanValue");
        int newValue = Mathf.Clamp(current + amount, 0, maxNpcBrwan);
        globalBlackboard.SetValue("npcBrwanValue", newValue);
        SaveAttributes();
        Debug.Log($"玩家体力变化: {current} -> {newValue}");
    }

    public void SetDogEmotion(string emotion)
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return;

        globalBlackboard.SetValue("dogEmotion", emotion);
        SaveAttributes();
        Debug.Log($"小狗心情设置为: {emotion}");
    }

    // ========== 设置特定值的方法 ==========

    public void SetDogEnergy(int value)
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return;

        int newValue = Mathf.Clamp(value, 0, maxDogEnergy);
        globalBlackboard.SetValue("dogEnergyValue", newValue);
        SaveAttributes();
        Debug.Log($"小狗精力值设置为: {newValue}");
    }

    public void SetDogHealth(int value)
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return;

        int newValue = Mathf.Clamp(value, 0, maxDogHealth);
        globalBlackboard.SetValue("dogHealthValue", newValue);
        SaveAttributes();
        Debug.Log($"小狗健康值设置为: {newValue}");
    }

    public void SetDogBelieve(int value)
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return;

        int newValue = Mathf.Clamp(value, 0, maxDogBelieve);
        globalBlackboard.SetValue("dogBelieveValue", newValue);
        SaveAttributes();
        Debug.Log($"小狗信任值设置为: {newValue}");
    }

    public void SetNpcCoin(int value)
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return;

        int newValue = Mathf.Max(0, value);
        globalBlackboard.SetValue("npcCoinValue", newValue);
        SaveAttributes();
        Debug.Log($"玩家金币设置为: {newValue}");
    }

    public void SetNpcBrwan(int value)
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return;

        int newValue = Mathf.Clamp(value, 0, maxNpcBrwan);
        globalBlackboard.SetValue("npcBrwanValue", newValue);
        SaveAttributes();
        Debug.Log($"玩家体力设置为: {newValue}");
    }

    // ========== 工具方法 ==========

    public void ResetAllAttributes()
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return;

        globalBlackboard.SetValue("dogEnergyValue", initialDogEnergy);
        globalBlackboard.SetValue("dogHealthValue", initialDogHealth);
        globalBlackboard.SetValue("dogBelieveValue", initialDogBelieve);
        globalBlackboard.SetValue("dogEmotion", initialDogEmotion);
        globalBlackboard.SetValue("npcCoinValue", initialNpcCoin);
        globalBlackboard.SetValue("npcBrwanValue", initialNpcBrwan);
        SaveAttributes();

        Debug.Log("所有属性已重置为初始值");
    }

    public void PrintAllAttributes()
    {
        Debug.Log("=== 当前属性值 ===");
        Debug.Log($"小狗精力值: {GetDogEnergy()}");
        Debug.Log($"小狗健康值: {GetDogHealth()}");
        Debug.Log($"小狗信任值: {GetDogBelieve()}");
        Debug.Log($"小狗心情: {GetDogEmotion()}");
        Debug.Log($"玩家金币: {GetNpcCoin()}");
        Debug.Log($"玩家体力: {GetNpcBrwan()}");
        Debug.Log("=================");
    }

    // ========== 获取属性值的方法 ==========

    private int GetDogEnergy()
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return initialDogEnergy;

        try
        {
            return globalBlackboard.GetValue<int>("dogEnergyValue");
        }
        catch (System.Exception)
        {
            return initialDogEnergy;
        }
    }

    private int GetDogHealth()
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return initialDogHealth;

        try
        {
            return globalBlackboard.GetValue<int>("dogHealthValue");
        }
        catch (System.Exception)
        {
            return initialDogHealth;
        }
    }

    private int GetDogBelieve()
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return initialDogBelieve;

        try
        {
            return globalBlackboard.GetValue<int>("dogBelieveValue");
        }
        catch (System.Exception)
        {
            return initialDogBelieve;
        }
    }

    private string GetDogEmotion()
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return initialDogEmotion;

        try
        {
            return globalBlackboard.GetValue<string>("dogEmotion");
        }
        catch (System.Exception)
        {
            return initialDogEmotion;
        }
    }

    private int GetNpcCoin()
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return initialNpcCoin;

        try
        {
            return globalBlackboard.GetValue<int>("npcCoinValue");
        }
        catch (System.Exception)
        {
            return initialNpcCoin;
        }
    }

    private int GetNpcBrwan()
    {
        if (globalBlackboard == null) FindGlobalBlackboard();
        if (globalBlackboard == null) return initialNpcBrwan;

        try
        {
            return globalBlackboard.GetValue<int>("npcBrwanValue");
        }
        catch (System.Exception)
        {
            return initialNpcBrwan;
        }
    }

    // ========== 保存/加载系统 ==========

    private void SaveAttributes()
    {
        if (globalBlackboard == null) return;

        PlayerPrefs.SetInt("dogEnergyValue", GetDogEnergy());
        PlayerPrefs.SetInt("dogHealthValue", GetDogHealth());
        PlayerPrefs.SetInt("dogBelieveValue", GetDogBelieve());
        PlayerPrefs.SetString("dogEmotion", GetDogEmotion());
        PlayerPrefs.SetInt("npcCoinValue", GetNpcCoin());
        PlayerPrefs.SetInt("npcBrwanValue", GetNpcBrwan());
        PlayerPrefs.Save();

        Debug.Log("游戏属性已保存");
    }

    private void LoadAttributes()
    {
        if (globalBlackboard == null) return;

        if (PlayerPrefs.HasKey("dogEnergyValue"))
        {
            globalBlackboard.SetValue("dogEnergyValue", PlayerPrefs.GetInt("dogEnergyValue", initialDogEnergy));
            globalBlackboard.SetValue("dogHealthValue", PlayerPrefs.GetInt("dogHealthValue", initialDogHealth));
            globalBlackboard.SetValue("dogBelieveValue", PlayerPrefs.GetInt("dogBelieveValue", initialDogBelieve));
            globalBlackboard.SetValue("dogEmotion", PlayerPrefs.GetString("dogEmotion", initialDogEmotion));
            globalBlackboard.SetValue("npcCoinValue", PlayerPrefs.GetInt("npcCoinValue", initialNpcCoin));
            globalBlackboard.SetValue("npcBrwanValue", PlayerPrefs.GetInt("npcBrwanValue", initialNpcBrwan));

            Debug.Log("存档数据已加载");
        }
        else
        {
            Debug.Log("无存档数据，使用初始值");
        }
    }
}
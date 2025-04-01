using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int playerEnergy = 45; // 玩家体力值
    public int playerCoins = 50; // 玩家金币
    public int dogHealth = 50; // 小狗体魄值
    public int dogEnergy = 50; // 小狗精力值
    public int dogTrust = 50; // 小狗信任值
    public string dogMood = "平静"; // 小狗心情

    public static PlayerStatus Instance { get; private set; } // 单例

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 跨场景保留
        }
        else
        {
            Debug.LogError("单例重复初始化: " + gameObject.name);
            Destroy(gameObject);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public Text hungerText; // 饱腹值文本
    public Text cleanlinessText; // 清洁值文本
    public Text healthText; // 健康值文本
    public Text energyText; // 精力值文本
    public Text moodText; // 心情值文本

    public float hunger = 80f; // 初始饱腹值
    public float cleanliness = 100f; // 初始清洁值
    public float health = 100f; // 初始健康值
    public float energy = 100f; // 初始精力值
    public float mood = 80f; // 初始心情值

    // 更新状态栏显示
    public void UpdateStatus()
    {
        hungerText.text = "饱腹值：" + hunger.ToString("F0") + "%"; // 更新饱腹值显示
        cleanlinessText.text = "清洁值：" + cleanliness.ToString("F0") + "%"; // 更新清洁值显示
        healthText.text = "健康值：" + health.ToString("F0") + "%"; // 更新健康值显示
        energyText.text = "精力值：" + energy.ToString("F0") + "%"; // 更新精力值显示
        moodText.text = "心情值：" + mood.ToString("F0") + "%"; // 更新心情值显示
    }
}
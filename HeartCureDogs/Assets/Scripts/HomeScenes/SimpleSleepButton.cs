using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodeCanvas.Framework;

public class SimpleSleepButton : MonoBehaviour
{
    private Button sleepButton;

    void Start()
    {
        sleepButton = GetComponent<Button>();
        if (sleepButton != null)
        {
            sleepButton.onClick.AddListener(OnSleepButtonClick);
        }
    }

    void OnSleepButtonClick()
    {
        Debug.Log("睡觉按钮被点击");

        // 设置全局黑板变量
        GlobalBlackboard gb = GlobalBlackboard.Find("Global");
        if (gb != null)
        {
            gb.SetVariableValue("sleepButtonPressed", true);
            Debug.Log("已设置 sleepButtonPressed = true");
        }
    }

    void OnDestroy()
    {
        if (sleepButton != null)
        {
            sleepButton.onClick.RemoveListener(OnSleepButtonClick);
        }
    }
}
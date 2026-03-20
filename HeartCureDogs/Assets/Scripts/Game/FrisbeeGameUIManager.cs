using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 飞盘游戏UI管理 - 快速启动器
/// 负责管理游戏UI的显示/隐藏，以及与其他系统的交互
/// </summary>
public class FrisbeeGameUIManager : MonoBehaviour
{
    [Header("引用")]
    [Tooltip("飞盘游戏管理器")]
    public FrisbeeGame frisbeeGame;
    [Tooltip("游戏UI容器（整个游戏界面）")]
    public GameObject gameUIContainer;
    [Tooltip("启动游戏按钮")]
    public Button startGameButton;

    void Start()
    {
        // 初始时隐藏游戏UI
        if (gameUIContainer != null)
            gameUIContainer.SetActive(false);

        // 绑定启动按钮事件
        if (startGameButton != null)
            startGameButton.onClick.AddListener(StartGame);
    }

    /// <summary>
    /// 启动小游戏
    /// </summary>
    public void StartGame()
    {
        if (gameUIContainer != null)
            gameUIContainer.SetActive(true);

        if (frisbeeGame != null)
            frisbeeGame.InitializeGame();
    }

    /// <summary>
    /// 退出小游戏
    /// </summary>
    public void ExitGame()
    {
        if (frisbeeGame != null)
            frisbeeGame.BackToPark();

        if (gameUIContainer != null)
            gameUIContainer.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 用于在 ParkScene1 场景中，通过按钮点击跳转到指定的小游戏场景。
/// </summary>
public class LoadMiniGameScene : MonoBehaviour
{
    [Tooltip("要加载的小游戏场景名称（需先在 Build Settings 中添加该场景）")]
    public string miniGameSceneName = "MiniGameScene";

    /// <summary>
    /// Button 的 OnClick 事件可以绑定此方法，用于加载指定的场景。
    /// </summary>
    public void LoadMiniGame()
    {
        if (string.IsNullOrEmpty(miniGameSceneName))
        {
            Debug.LogWarning("未设置 miniGameSceneName，请在 Inspector 中指定要加载的场景名称。");
            return;
        }

        SceneManager.LoadScene(miniGameSceneName);
    }
}

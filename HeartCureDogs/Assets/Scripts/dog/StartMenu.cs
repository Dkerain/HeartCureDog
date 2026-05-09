using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [Header("场景名称")]
    public string chooseDogSceneName = "ChooseDogScene"; // 选狗场景名称

    public void OnStartGame()
    {
        Debug.Log("开始游戏，进入选狗场景");
        SceneManager.LoadScene(chooseDogSceneName);
    }

    public void OnExitGame()
    {
        Debug.Log("退出游戏");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 编辑器模式下停止运行
#else
        Application.Quit(); // 打包后退出应用
#endif
    }

    // 可选：如果添加了设置按钮，可以调用这个方法
    public void OnSettings()
    {
        Debug.Log("打开设置面板（暂未实现）");
        // 例如：打开一个设置弹窗，或加载设置场景
    }
}
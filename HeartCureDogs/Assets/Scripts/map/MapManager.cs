using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public GameObject mapPanel; // 拖入MapPanel

    public GameObject sceneButtonPrefab; // 拖入按钮预制体

    void Start()
    {
        // 初始时关闭地图
        mapPanel.SetActive(false);
    }

    // 点击地图图标时调用
    public void ToggleMap()
    {
        bool isOpen = mapPanel.activeSelf;
        mapPanel.SetActive(!isOpen);
        Time.timeScale = isOpen ? 1 : 0; // 暂停游戏
    }

    // 场景按钮点击时的通用方法
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1; // 恢复时间
        if (mapPanel != null)
            mapPanel.SetActive(false);
        SceneManager.LoadScene(sceneName);
    }

    // 新增控制方法
    public void CloseMap()
    {
        mapPanel.SetActive(false);
        Time.timeScale = 1; // 恢复游戏时间
    }

}



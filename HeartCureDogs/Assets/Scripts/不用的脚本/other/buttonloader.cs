using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 引用UI命名空间以访问Button类

public class ButtonLoader : MonoBehaviour
{
    public string sceneName; // 要跳转的场景名称
    public Button button;//要有跳转功能的按钮
    void Start()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
        if (button != null)
        {
            button.onClick.AddListener(LoadScene);
        }
        //// 获取按钮组件
        //Button button = GetComponent<Button>();

        //// 为按钮添加点击事件监听器
        //button.onClick.AddListener(LoadScene);
    }
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("加载场景失败，场景为空");
        }
    }
}
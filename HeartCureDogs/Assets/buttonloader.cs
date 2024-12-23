using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 引用UI命名空间以访问Button类

public class ButtonLoader : MonoBehaviour
{
    public string sceneName; // 要跳转的场景名称

    void Start()
    {
        // 获取按钮组件
        Button button = GetComponent<Button>();

        // 为按钮添加点击事件监听器
        button.onClick.AddListener(LoadScene);
    }

    public void LoadScene()
    {
        // 点击时加载指定场景
        SceneManager.LoadScene(sceneName);
    }
}
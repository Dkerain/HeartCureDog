using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance; // 简单的单例模式，方便访问

    private Stack<GameObject> interfaceStack = new Stack<GameObject>(); // 界面栈

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 可选：跨场景不销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 打开一个新界面并压入栈
    public void PushInterface(GameObject newInterface)
    {
        // 如果栈中有界面，先隐藏它
        if (interfaceStack.Count > 0)
        {
            interfaceStack.Peek().SetActive(false);
        }

        // 显示新界面并压栈
        newInterface.SetActive(true);
        interfaceStack.Push(newInterface);
    }

    // 关闭当前界面并返回上一个界面
    public void PopInterface()
    {
        // 如果栈中界面少于2个，则无法返回（至少有一个当前界面和一个目标界面）
        if (interfaceStack.Count < 2)
        {
            Debug.LogWarning("Interface stack is too small to pop.");
            return;
        }

        // 弹出并关闭当前界面
        GameObject currentInterface = interfaceStack.Pop();
        currentInterface.SetActive(false);

        // 弹出并打开上一个界面
        GameObject previousInterface = interfaceStack.Peek();
        previousInterface.SetActive(true);
    }
}
public class BackButtonWithStack : MonoBehaviour
{
    private Button backButton;

    void Start()
    {
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnBackButtonClick()
    {
        // 直接调用界面管理器的方法
        InterfaceManager.Instance.PopInterface();
    }
}

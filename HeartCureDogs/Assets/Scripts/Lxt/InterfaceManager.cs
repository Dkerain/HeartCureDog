using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;

    private Stack<GameObject> interfaceStack = new Stack<GameObject>();

    // 添加一个字段来指定初始界面
    public GameObject initialInterface;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 在Start中将初始界面压入栈
        if (initialInterface != null)
        {
            PushInterface(initialInterface);
        }
        else
        {
            Debug.LogWarning("请为InterfaceManager指定初始界面");
        }
    }

    // 打开一个新界面并压入栈
    public void PushInterface(GameObject newInterface)
    {
        // 如果栈中有界面，先隐藏它
        if (interfaceStack.Count > 0)
        {
            GameObject currentInterface = interfaceStack.Peek();
            currentInterface.SetActive(false);
            Debug.Log("隐藏界面: " + currentInterface.name);
        }

        // 显示新界面并压栈
        newInterface.SetActive(true);
        interfaceStack.Push(newInterface);
        Debug.Log("显示界面: " + newInterface.name + ", 当前栈大小: " + interfaceStack.Count);
    }

    // 关闭当前界面并返回上一个界面
    public void PopInterface()
    {
        // 如果栈中界面少于2个，则无法返回
        if (interfaceStack.Count < 2)
        {
            Debug.LogWarning("Interface stack is too small to pop. Current count: " + interfaceStack.Count);
            return;
        }

        // 弹出并关闭当前界面
        GameObject currentInterface = interfaceStack.Pop();
        currentInterface.SetActive(false);
        Debug.Log("关闭界面: " + currentInterface.name);

        // 弹出并打开上一个界面
        GameObject previousInterface = interfaceStack.Peek();
        previousInterface.SetActive(true);
        Debug.Log("打开界面: " + previousInterface.name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        Debug.Log("存档界面返回按钮点击");
        
        if (InterfaceManager.Instance != null)
        {
            InterfaceManager.Instance.PopInterface();
        }
        else
        {
            Debug.LogError("InterfaceManager 未找到！");
            // 备用方案：直接禁用当前界面
            transform.parent.gameObject.SetActive(false);
        }
    }
}

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
        // 直接调用界面管理器的方法
        InterfaceManager.Instance.PopInterface();
    }
}

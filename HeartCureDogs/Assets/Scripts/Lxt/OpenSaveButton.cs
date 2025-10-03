using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenSettingsButton : MonoBehaviour
{
    public GameObject settingsPanel; // 在Inspector中拖拽SettingsPanel到这里

    private Button settingsButton;

    void Start()
    {
        settingsButton = GetComponent<Button>();
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
    }

    private void OnSettingsButtonClick()
    {
        if (settingsPanel != null && InterfaceManager.Instance != null)
        {
            InterfaceManager.Instance.PushInterface(settingsPanel);
        }
    }
}

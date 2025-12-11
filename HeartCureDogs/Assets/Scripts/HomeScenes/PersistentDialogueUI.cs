using UnityEngine;
using NodeCanvas.DialogueTrees.UI.Examples;

public class PersistentDialogueUI : MonoBehaviour
{
    private static PersistentDialogueUI _instance;

    private void Awake()
    {
        // 单例模式，确保只有一个对话UI
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("对话UI设置为跨场景不销毁");
    }

    private void OnEnable()
    {
        // 确保对话UI组件启用
        DialogueUGUI dialogueUI = GetComponent<DialogueUGUI>();
        if (dialogueUI != null && !dialogueUI.enabled)
        {
            dialogueUI.enabled = true;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText; // 对话文本组件
    public string[] dialogues; // 对话内容数组
    private int currentDialogueIndex = 0; // 当前对话索引

    // 显示对话
    public void ShowDialogue(string dialogue)
    {
        dialogueText.text = dialogue; // 设置对话文本
        gameObject.SetActive(true); // 显示对话框
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombinedDialogueSystem : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI dialogText; // 对话文本组件
    public Button nextButton; // 下一句按钮
    public Image roleImage; // 角色图片组件
    public TextMeshProUGUI roleNameText; // 角色名字文本组件

    [Header("音频反馈")]
    public AudioClip typingSound; // 打字音效
    public AudioClip continueSound; // 点击按钮音效

    [Header("对话数据")]
    public TextAsset dialogDataFile; // 对话文本文件，CSV 格式

    private Queue<DialogueEntry> sentences = new Queue<DialogueEntry>(); // 存储对话条目的队列
    private bool isTyping; // 标记是否正在打字
    private AudioSource audioSource; // 音频源组件
    private Dictionary<string, Sprite> roleSpritesDic = new Dictionary<string, Sprite>(); // 角色名字与图片的字典
    public List<Sprite> roleSprites = new List<Sprite>(); // 角色图片列表

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 手动检查并绑定按钮事件
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(OnNextButtonClicked);
        }
        else
        {
            Debug.LogError("Next按钮未绑定！");
        }

        // 初始化角色图片字典
        roleSpritesDic["印小棠"] = roleSprites[0];
        roleSpritesDic["旁白"] = roleSprites[1];
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeUIComponents();
    }

    private void InitializeUIComponents()
    {
        // 手动查找所需组件（确保每场景都有正确命名）
        if (dialogText == null)
            dialogText = GameObject.Find("DialogText")?.GetComponent<TextMeshProUGUI>();

        if (nextButton == null)
            nextButton = GameObject.Find("NextButton")?.GetComponent<Button>();

        if (roleImage == null)
            roleImage = GameObject.Find("RoleImage")?.GetComponent<Image>();

        if (roleNameText == null)
            roleNameText = GameObject.Find("RoleNameText")?.GetComponent<TextMeshProUGUI>();

        if (dialogText == null || nextButton == null || roleImage == null || roleNameText == null)
        {
            Debug.LogError("UI组件初始化失败，请检查场景中的DialogText、NextButton、RoleImage和RoleNameText对象");
            enabled = false;
        }

        nextButton.gameObject.SetActive(false);
    }

    public bool IsDialogueActive()
    {
        return sentences.Count > 0 || isTyping;
    }

    public void StartDialogue(List<DialogueEntry> dialogues)
    {
        sentences.Clear();
        foreach (DialogueEntry entry in dialogues) sentences.Enqueue(entry);
        nextButton.gameObject.SetActive(true);
        ShowNextSentence();
    }

    public void OnNextButtonClicked()
    {
        if (audioSource && continueSound)
            audioSource.PlayOneShot(continueSound);
        ShowNextSentence();
    }

    private void ShowNextSentence()
    {
        if (isTyping) return;

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        StartCoroutine(TypeSentence(sentences.Dequeue()));
    }

    IEnumerator TypeSentence(DialogueEntry entry)
    {
        isTyping = true;
        dialogText.text = "";
        nextButton.interactable = false;

        roleNameText.text = entry.roleName;
        roleImage.sprite = roleSpritesDic[entry.roleName];

        foreach (char letter in entry.dialogue)
        {
            dialogText.text += letter;
            if (typingSound && audioSource)
                audioSource.PlayOneShot(typingSound);
            yield return new WaitForSeconds(0.05f);
        }

        isTyping = false;
        nextButton.interactable = sentences.Count > 0;
    }

    private void EndDialogue()
    {
        sentences.Clear();
        nextButton.gameObject.SetActive(false);
    }

    public void LoadDialogueData(string startIndex, string endIndex)
    {
        if (dialogDataFile == null)
        {
            Debug.LogError("未指定对话数据文件！");
            return;
        }

        int start, end;
        if (!int.TryParse(startIndex, out start) || !int.TryParse(endIndex, out end))
        {
            Debug.LogError("Invalid start or end index format: " + startIndex + " or " + endIndex);
            return;
        }

        string[] dialogueRows = dialogDataFile.text.Split('\n');
        List<DialogueEntry> dialogues = new List<DialogueEntry>();

        for (int i = start; i <= end && i < dialogueRows.Length; i++)
        {
            string[] cells = dialogueRows[i].Split(',');
            if (cells.Length >= 4)
            {
                int nextIndex;
                if (!int.TryParse(cells[4], out nextIndex))
                {
                    Debug.LogError("Invalid next index format: " + cells[4] + " in row: " + i);
                    continue; // 跳过当前行
                }

                DialogueEntry entry = new DialogueEntry
                {
                    roleName = cells[2],
                    dialogue = cells[3],
                    nextIndex = nextIndex
                };
                dialogues.Add(entry);
            }
        }

        if (dialogues.Count == 0)
        {
            Debug.LogError("No dialogue entries found from index: " + startIndex + " to " + endIndex);
        }

        StartDialogue(dialogues);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

[System.Serializable]
public class DialogueEntry
{
    public string roleName; // 角色名字
    public string dialogue; // 对话内容
    public int nextIndex; // 下一句索引
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI dialogText;
    public Button nextButton; // 改为直接绑定按钮

    [Header("音频反馈")]
    public AudioClip typingSound;
    public AudioClip continueSound;

    private Queue<string> sentences = new Queue<string>();
    private bool isTyping;
    private AudioSource audioSource;

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

        if (dialogText == null || nextButton == null)
        {
            Debug.LogError("UI组件初始化失败，请检查场景中的DialogText和NextButton对象");
            enabled = false;
        }

        nextButton.gameObject.SetActive(false);
    }

    public bool IsDialogueActive()
    {
        return sentences.Count > 0 || isTyping;
    }

    public void StartDialogue(List<string> dialogues)
    {
        sentences.Clear();
        foreach (string line in dialogues) sentences.Enqueue(line);
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

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogText.text = "";
        nextButton.interactable = false;

        foreach (char letter in sentence)
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

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

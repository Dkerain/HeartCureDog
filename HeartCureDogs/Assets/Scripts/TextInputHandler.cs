using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DogUnity;
using System.Linq;

public class TextInputHandler : MonoBehaviour
{
    [SerializeField] public TMP_InputField playerInputField;
    [SerializeField] private TMP_Text glmText;
    public Image DialogueImage;   // 气泡背景图（或整个气泡面板的根节点Image）
    public GameObject bubblePanel; // 气泡的根物体（包含背景和文本的Panel/Canvas）

    [SerializeField, Tooltip("玩家与GLM的对话历史记录，List<SendData>格式存储")]
    private List<SendData> chatHistory = new List<SendData>();

    private Coroutine hideBubbleCoroutine;

    void Start()
    {
        // 系统提示，设定AI角色
        SendData systemPrompt = new SendData()
        {
            role = "system",
            content = "你是一只可爱的柯基犬，名字叫墩墩。"
        };
        chatHistory.Add(systemPrompt);

        // 初始时隐藏整个气泡面板
        if (bubblePanel != null)
            bubblePanel.SetActive(false);
        else
            Debug.LogWarning("bubblePanel 未赋值，请将对话气泡的根物体拖拽到此字段");
    }

    public async void SendPlayerResponse()
    {
        // 获取玩家输入
        string playerInput = playerInputField.text;
        if (string.IsNullOrWhiteSpace(playerInput))
            return;

        playerInputField.text = "";

        // 添加玩家消息到历史
        SendData playerMessage = new SendData()
        {
            role = "user",
            content = playerInput
        };
        chatHistory.Add(playerMessage);

        // 调用AI接口获取回复
        SendData response = await DogHandler.GenerateDogResponse(chatHistory, 0.8f);
        chatHistory.Add(response);

        // 更新UI文本
        if (glmText != null)
            glmText.text = response.content;

        // 显示气泡
        ShowBubble();

        // 如果已有倒计时，重置计时器
        if (hideBubbleCoroutine != null)
            StopCoroutine(hideBubbleCoroutine);

        // 开始5秒后隐藏
        hideBubbleCoroutine = StartCoroutine(HideBubbleAfterDelay(15f));
    }

    private void ShowBubble()
    {
        if (bubblePanel != null)
            bubblePanel.SetActive(true);
        else if (DialogueImage != null)
            DialogueImage.gameObject.SetActive(true); // 兼容旧的单Image引用
        else
            Debug.LogWarning("无法显示气泡：bubblePanel未赋值");
    }

    private IEnumerator HideBubbleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (bubblePanel != null)
            bubblePanel.SetActive(false);
        else if (DialogueImage != null)
            DialogueImage.gameObject.SetActive(false);
        hideBubbleCoroutine = null;
    }
}
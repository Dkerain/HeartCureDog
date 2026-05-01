using UnityEngine;
using NodeCanvas.Framework;
using System.Collections;

public class DogEmotionBubble : MonoBehaviour
{
    [System.Serializable]
    public class EmojiMapping
    {
        public string emotionName;   // 与黑板 dogEmotion 的值一致
        public GameObject emojiImage; // 对应的表情图片（位于气泡 Canvas 下）
    }

    [Header("气泡物体（小狗的子物体）")]
    public GameObject bubbleCanvas;           // 气泡 Canvas 物体（初始隐藏）

    [Header("表情映射")]
    public EmojiMapping[] emojiMappings;

    [Header("显示设置")]
    public float displayDuration = 3f;      // 显示时长（秒）

    private Blackboard globalBlackboard;
    private string lastEmotion;                // 记录上次情绪，避免重复触发
    private Coroutine hideCoroutine;

    void Start()
    {
        // 1. 获取全局黑板
        globalBlackboard = GlobalBlackboard.Find("Global");
        if (globalBlackboard == null)
        {
            Debug.LogError("DogEmotionBubble: 找不到标识符为 'Global' 的 GlobalBlackboard！");
            return;
        }

        // 2. 确保初始无气泡显示
        if (bubbleCanvas != null)
            bubbleCanvas.SetActive(false);
        else
            Debug.LogError("DogEmotionBubble: bubbleCanvas 未赋值！");

        // 3. 读取初始情绪，但不显示气泡（避免开场突兀）
        lastEmotion = globalBlackboard.GetValue<string>("dogEmotion");
    }

    void Update()
    {
        if (globalBlackboard == null) return;

        string currentEmotion = globalBlackboard.GetValue<string>("dogEmotion");
        // 只有当情绪值发生变化时才显示气泡
        if (currentEmotion != lastEmotion)
        {
            lastEmotion = currentEmotion;
            ShowEmoji(currentEmotion);
        }
    }

    private void ShowEmoji(string emotionName)
    {
        if (bubbleCanvas == null) return;

        // 停止当前的隐藏协程，并立即隐藏气泡（重置计时）
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            bubbleCanvas.SetActive(false);
        }

        // 查找对应的表情图片
        GameObject targetEmoji = null;
        foreach (var mapping in emojiMappings)
        {
            if (mapping.emotionName == emotionName)
            {
                targetEmoji = mapping.emojiImage;
                break;
            }
        }

        if (targetEmoji == null)
        {
            Debug.LogWarning($"DogEmotionBubble: 未找到心情 '{emotionName}' 对应的 Emoji 图片");
            return;
        }

        // 隐藏所有表情图片
        foreach (var mapping in emojiMappings)
        {
            if (mapping.emojiImage != null)
                mapping.emojiImage.SetActive(false);
        }
        targetEmoji.SetActive(true);

        // 显示气泡 Canvas
        bubbleCanvas.SetActive(true);

        // 开始倒计时隐藏
        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        if (bubbleCanvas != null)
            bubbleCanvas.SetActive(false);
        hideCoroutine = null;
    }
}
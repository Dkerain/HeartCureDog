using UnityEngine;
using NodeCanvas.Framework;
using TMPro; // 如果使用 TextMeshPro，否则改用 UnityEngine.UI

[RequireComponent(typeof(SpriteRenderer))]
public class DogEmotionController : MonoBehaviour
{
    [System.Serializable]
    public class EmotionSprite
    {
        public string emotionName;   // 与黑板变量 dogEmotion 的值保持一致
        public Sprite sprite;
    }

    [Header("表情配置")]
    public EmotionSprite[] emotionSprites;

    [Header("Emoji 气泡（可选）")]
    public GameObject emojiBubblePrefab;   // 预制体：一个带背景和文字的 Canvas（World Space）
    public float emojiDisplayDuration = 1.5f;

    private SpriteRenderer spriteRenderer;
    private Blackboard globalBlackboard;
    private DogControllerr dogController;   // 引用移动控制器，判断是否移动
    private string currentEmotion;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("DogEmotionController: 未找到 SpriteRenderer 组件！");
            return;
        }

        // 获取移动控制脚本（注意：你的脚本名是 DogControllerr，不是 DogMovement）
        dogController = GetComponent<DogControllerr>();
        if (dogController == null)
        {
            Debug.LogWarning("DogEmotionController: 未找到 DogControllerr 组件，将无法区分移动/静止状态，表情将在所有状态下覆盖。");
        }

        // 查找全局黑板（标识符 "Global"）
        globalBlackboard = GlobalBlackboard.Find("Global");
        if (globalBlackboard == null)
        {
            Debug.LogError("DogEmotionController: 找不到标识符为 'Global' 的 GlobalBlackboard！");
            return;
        }

        // 初始应用表情
        UpdateEmotionFromBlackboard();
    }

    void LateUpdate()
    {
        // 如果小狗正在移动，不覆盖精灵（让 Animator 控制行走动画）
        if (dogController != null && dogController.IsMoving)
            return;

        // 静止状态下，强制应用当前情绪精灵
        if (globalBlackboard != null && spriteRenderer != null)
        {
            string targetEmotion = globalBlackboard.GetValue<string>("dogEmotion");
            SetSpriteForEmotion(targetEmotion);
        }
    }

    // 从黑板读取 dogEmotion 并刷新精灵（供外部手动调用）
    public void UpdateEmotionFromBlackboard()
    {
        if (globalBlackboard == null || spriteRenderer == null) return;
        string emotion = globalBlackboard.GetValue<string>("dogEmotion");
        SetSpriteForEmotion(emotion);
    }

    // 设置精灵（内部使用）
    private void SetSpriteForEmotion(string emotionName)
    {
        foreach (var item in emotionSprites)
        {
            if (item.emotionName == emotionName)
            {
                if (spriteRenderer.sprite != item.sprite)
                {
                    spriteRenderer.sprite = item.sprite;
                    Debug.Log($"小狗表情已切换为: {emotionName}");
                }
                currentEmotion = emotionName;
                return;
            }
        }
        Debug.LogWarning($"DogEmotionController: 没有找到名为 '{emotionName}' 的表情配置，请检查 Inspector");
    }

    // 直接修改黑板中的心情（可选）
    public void SetEmotion(string emotionName)
    {
        if (globalBlackboard != null)
        {
            globalBlackboard.SetValue("dogEmotion", emotionName);
            // 如果是静止状态，立即应用
            if (dogController != null && !dogController.IsMoving)
                SetSpriteForEmotion(emotionName);
        }
    }

    // ========== Emoji 气泡功能 ==========
    public void ShowEmoji(string emoji)
    {
        if (emojiBubblePrefab != null)
        {
            // 使用预制体实例化气泡
            Vector3 spawnPos = transform.position + Vector3.up * 1.2f; // 头顶偏移
            GameObject bubble = Instantiate(emojiBubblePrefab, spawnPos, Quaternion.identity, transform);
            var textComp = bubble.GetComponentInChildren<TMP_Text>();
            if (textComp != null)
                textComp.text = emoji;
            // 如果预制体是 Sprite 方式，可以再处理
            Destroy(bubble, emojiDisplayDuration);
        }
        else
        {
            // 简易方式：动态创建文本
            ShowSimpleEmojiText(emoji);
        }
    }

    private void ShowSimpleEmojiText(string emoji)
    {
        GameObject go = new GameObject("EmojiBubble");
        go.transform.position = transform.position + Vector3.up * 1.2f;
        go.transform.SetParent(transform);
        var textMesh = go.AddComponent<TextMesh>();
        textMesh.text = emoji;
        textMesh.fontSize = 50;
        textMesh.characterSize = 0.1f;
        textMesh.alignment = TextAlignment.Center;
        // 添加一个简单的背景（可选）
        Destroy(go, emojiDisplayDuration);
    }
}
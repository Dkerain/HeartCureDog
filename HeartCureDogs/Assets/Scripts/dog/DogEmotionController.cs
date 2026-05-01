using UnityEngine;
using NodeCanvas.Framework;

[RequireComponent(typeof(SpriteRenderer))]
public class DogEmotionController : MonoBehaviour
{
    [System.Serializable]
    public class EmotionSprite
    {
        public string emotionName;   // 与黑板变量 dogEmotion 的值保持一致
        public Sprite sprite;
    }

   
    public EmotionSprite[] emotionSprites;
    private SpriteRenderer spriteRenderer;
    private Blackboard globalBlackboard;
    private DogControllerr dogController;   // 引用小狗移动控制器
    private string currentEmotion;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("DogEmotionController: 未找到 SpriteRenderer 组件！");
            return;
        }

        // 获取小狗移动控制器
        dogController = GetComponent<DogControllerr>();
        if (dogController == null)
        {
            Debug.LogError("DogEmotionController: 未找到 DogControllerr 组件！将无法区分移动/静止状态。");
        }

        // 找到全局黑板（标识符为 "Global"）
        globalBlackboard = GlobalBlackboard.Find("Global");
        if (globalBlackboard == null)
        {
            Debug.LogError("DogEmotionController: 找不到标识符为 'Global' 的 GlobalBlackboard！");
            return;
        }

        // 初始化时立即应用当前情绪（静止状态）
        UpdateEmotionFromBlackboard();
    }

    void LateUpdate()
    {
        // 如果小狗正在移动，不覆盖精灵（让 Animator 控制行走动画）
        if (dogController != null && dogController.IsMoving)
            return;

        // 静止状态下，每帧强制应用当前情绪对应的精灵（覆盖可能的 idle 动画）
        if (globalBlackboard != null && spriteRenderer != null)
        {
            string targetEmotion = globalBlackboard.GetValue<string>("dogEmotion");
            SetSpriteForEmotion(targetEmotion);
        }
    }

    // 从黑板读取 dogEmotion 并刷新精灵
    public void UpdateEmotionFromBlackboard()
    {
        if (globalBlackboard == null || spriteRenderer == null) return;
        string emotion = globalBlackboard.GetValue<string>("dogEmotion");
        SetSpriteForEmotion(emotion);
    }

    // 核心：设置精灵，如果已经相同则跳过
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

    // 可选：直接修改黑板中的 dogEmotion，表情会自动更新
    public void SetEmotion(string emotionName)
    {
        if (globalBlackboard != null)
        {
            globalBlackboard.SetValue("dogEmotion", emotionName);
            // 如果是静止状态，立即应用；移动状态则无需立即应用，等静止时自动应用
            if (dogController != null && !dogController.IsMoving)
                SetSpriteForEmotion(emotionName);
        }
    }
}
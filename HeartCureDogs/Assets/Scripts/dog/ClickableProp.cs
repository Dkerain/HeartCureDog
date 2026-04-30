using UnityEngine;
using TMPro;          // 引入 TMPro 命名空间
using System.Collections;

public class ClickableProp : MonoBehaviour
{
    [Header("物品信息")]
    public string propName = "物品";

    [Header("属性变化值（正数为增加，负数为减少）")]
    public int dogHealthDelta = 0;
    public int dogEnergyDelta = 0;
    public int dogBelieveDelta = 0;
    public int npcCoinDelta = 0;
    public int npcBrwanDelta = 0;
    public string dogEmotion = "";

    [Header("反馈效果")]
    public string floatingText = "";
    public string emoji = "😀";

    [Header("点击后是否销毁物品")]
    public bool destroyAfterClick = false;

    private DogAttributeManager attrManager;
    private GameObject dog;
    private DogEmotionController dogEmotionController;

    void Start()
    {
        attrManager = FindObjectOfType<DogAttributeManager>();
        if (attrManager == null)
            Debug.LogError("ClickableProp: 未找到 DogAttributeManager！");

        dog = GameObject.FindGameObjectWithTag("PlayerDog");
        if (dog == null)
            dog = GameObject.Find("Dog");

        if (dog != null)
            dogEmotionController = dog.GetComponent<DogEmotionController>();
    }

    void OnMouseDown()
    {
        if (attrManager == null) return;

        // 1. 应用属性变化
        if (dogHealthDelta != 0)
            attrManager.AddDogHealth(dogHealthDelta);
        if (dogEnergyDelta != 0)
            attrManager.AddDogEnergy(dogEnergyDelta);
        if (dogBelieveDelta != 0)
            attrManager.AddDogBelieve(dogBelieveDelta);
        if (npcCoinDelta != 0)
            attrManager.AddNpcCoin(npcCoinDelta);
        if (npcBrwanDelta != 0)
            attrManager.AddNpcBrwan(npcBrwanDelta);
        if (!string.IsNullOrEmpty(dogEmotion))
            attrManager.SetDogEmotion(dogEmotion);

        // 2. 显示浮动提示条
        string tipText = string.IsNullOrEmpty(floatingText) ? GenerateTipText() : floatingText;
        ShowFloatingTip(tipText);

        // 3. 小狗头顶 emoji
        if (dogEmotionController != null)
            dogEmotionController.ShowEmoji(emoji);

        // 4. 可选：销毁物品
        if (destroyAfterClick)
            Destroy(gameObject);
    }

    private string GenerateTipText()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (dogHealthDelta != 0) sb.Append($"体魄{(dogHealthDelta > 0 ? "+" : "")}{dogHealthDelta} ");
        if (dogEnergyDelta != 0) sb.Append($"精力{(dogEnergyDelta > 0 ? "+" : "")}{dogEnergyDelta} ");
        if (dogBelieveDelta != 0) sb.Append($"信任{(dogBelieveDelta > 0 ? "+" : "")}{dogBelieveDelta} ");
        if (npcCoinDelta != 0) sb.Append($"金币{(npcCoinDelta > 0 ? "+" : "")}{npcCoinDelta} ");
        if (npcBrwanDelta != 0) sb.Append($"体力{(npcBrwanDelta > 0 ? "+" : "")}{npcBrwanDelta} ");
        if (!string.IsNullOrEmpty(dogEmotion)) sb.Append($"心情变为{dogEmotion}");
        return sb.ToString().Trim();
    }

    private void ShowFloatingTip(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("未找到 Canvas，无法显示提示条！");
            return;
        }

        // 使用 TextMeshProUGUI
        GameObject floatingObj = new GameObject("FloatingTip");
        floatingObj.transform.SetParent(canvas.transform, false);
        // 将屏幕坐标设置为物品上方
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);
        floatingObj.transform.position = screenPos;

        TextMeshProUGUI textComp = floatingObj.AddComponent<TextMeshProUGUI>();
        textComp.text = text;
        textComp.fontSize = 24;
        textComp.color = Color.yellow;
        textComp.outlineWidth = 0.2f;
        textComp.outlineColor = Color.black;

        // 2秒后自动销毁
        Destroy(floatingObj, 2f);
    }
}
using UnityEngine;
using System.Text;

[RequireComponent(typeof(Collider2D))]
public class ClickableProp : MonoBehaviour
{
    [Header("物品信息")]
    public string propName = "物品";

    [Header("属性变化（正数增加，负数减少）")]
    public int dogHealthDelta = 0;
    public int dogEnergyDelta = 0;
    public int dogBelieveDelta = 0;
    public int npcCoinDelta = 0;
    public int npcBrwanDelta = 0;
    public string setDogEmotion = "";   // 例如 "开心"，会调用 SetDogEmotion

    [Header("提示条")]
    public string customTipText = "";    // 留空则自动生成
    public float tipDuration = 4f;

    [Header("点击后行为")]
    public bool destroyAfterClick = false;

    private DogAttributeManager attrManager;

    private void Start()
    {
        attrManager = FindObjectOfType<DogAttributeManager>();
        if (attrManager == null)
            Debug.LogError($"ClickableProp {propName}: 未找到 DogAttributeManager！");
    }

    private void OnMouseDown()
    {
        if (attrManager == null) return;

        // 1. 修改属性
        if (dogHealthDelta != 0) attrManager.AddDogHealth(dogHealthDelta);
        if (dogEnergyDelta != 0) attrManager.AddDogEnergy(dogEnergyDelta);
        if (dogBelieveDelta != 0) attrManager.AddDogBelieve(dogBelieveDelta);
        if (npcCoinDelta != 0) attrManager.AddNpcCoin(npcCoinDelta);
        if (npcBrwanDelta != 0) attrManager.AddNpcBrwan(npcBrwanDelta);
        if (!string.IsNullOrEmpty(setDogEmotion)) attrManager.SetDogEmotion(setDogEmotion);

        // 2. 显示浮动提示条
        string tip = string.IsNullOrEmpty(customTipText) ? GenerateTipText() : customTipText;
        ShowFloatingTip(tip);

        // 3. 点击后销毁
        if (destroyAfterClick) Destroy(gameObject);
    }

    private string GenerateTipText()
    {
        StringBuilder sb = new StringBuilder();
        if (dogHealthDelta != 0) sb.Append($"体魄{(dogHealthDelta > 0 ? "+" : "")}{dogHealthDelta} ");
        if (dogEnergyDelta != 0) sb.Append($"精力{(dogEnergyDelta > 0 ? "+" : "")}{dogEnergyDelta} ");
        if (dogBelieveDelta != 0) sb.Append($"信任{(dogBelieveDelta > 0 ? "+" : "")}{dogBelieveDelta} ");
        if (npcCoinDelta != 0) sb.Append($"金币{(npcCoinDelta > 0 ? "+" : "")}{npcCoinDelta} ");
        if (npcBrwanDelta != 0) sb.Append($"体力{(npcBrwanDelta > 0 ? "+" : "")}{npcBrwanDelta} ");
        if (!string.IsNullOrEmpty(setDogEmotion)) sb.Append($"心情→{setDogEmotion}");
        return sb.ToString().Trim();
    }

    private void ShowFloatingTip(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        // 创建 Canvas
        GameObject canvasObj = new GameObject("FloatingTipCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.sortingOrder = 3;   // 图层 = 3

        // 可选：添加 CanvasScaler 和 GraphicRaycaster（不需要）

        // 设置 Canvas 位置为物品上方
        canvasObj.transform.position = transform.position + Vector3.up * 1.2f;
        // 让 Canvas 始终面向摄像机（如果是世界空间且需要面向摄像机，可以取消注释）
        // canvasObj.transform.LookAt(Camera.main.transform);
        // 或者让 Canvas 固定方向，自己调整

        // 添加 TextMeshProUGUI
        var tmp = canvasObj.AddComponent<TMPro.TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 14;      // 大小 50
        tmp.color = Color.black; // 黑色字体
        tmp.alignment = TMPro.TextAlignmentOptions.Center;

        // 调整 Canvas 缩放，使文字大小适中（根据你的世界单位，可能需要调整）
        canvasObj.transform.localScale = Vector3.one * 0.01f; // 根据实际场景调整

        // 自动销毁
        Destroy(canvasObj, tipDuration);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    public Image backgroundImage;
    public Sprite[] backgroundSprites;
    // 用名字映射更方便
    public void ChangeBackground(string bgName)
    {
        Sprite target = System.Array.Find(backgroundSprites, s => s.name == bgName);
        if (target != null)
            backgroundImage.sprite = target;
    }

    // 或者用索引
    public void ChangeBackgroundByIndex(int index)
    {
        if (index >= 0 && index < backgroundSprites.Length)
            backgroundImage.sprite = backgroundSprites[index];
    }
}
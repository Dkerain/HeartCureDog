using UnityEngine;
using UnityEngine.UI;

public class DogImageManager : MonoBehaviour
{
    public Image dogImage;          // 显示小狗的 UI Image
    public Sprite[] dogSprites;     // 按顺序放入：0=正常,1=开心,2=伤心,3=紧张,4=兴奋...

    public void SetDogImage(int index)
    {
        if (dogImage != null && index >= 0 && index < dogSprites.Length)
        {
            dogImage.sprite = dogSprites[index];
        }
    }

    // 如果喜欢用字符串名字，可以加一个字典映射
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    // 声明UI文本组件
    public Text dogInfoText;
    public Text npcInfoText;

    // 角色实例
    private Dogs myDog;
    private NPCs mainCharacter;

    void Start()
    {
        // 创建墩墩狗狗实例
        myDog = new Dogs
        {
            name = "墩墩",
            emotion = "平静",
            healthValue = 50,
            believeValue = 50,
            energyValue = 50
        };

        // 创建NPC主角实例
        mainCharacter = new NPCs
        {
            name = "主角",
            coinValue = 50,
            brwanValue = 45
        };

        // 更新UI显示
        UpdateCharacterUI();
    }

    void UpdateCharacterUI()
    {
        if (dogInfoText != null)
        {
            dogInfoText.text = $"名字: {myDog.name}\n" +
                              $"心情: {myDog.emotion}\n" +
                              $"健康值: {myDog.healthValue}\n" +
                              $"信任值: {myDog.believeValue}\n" +
                              $"精力值: {myDog.energyValue}";
        }

        if (npcInfoText != null)
        {
            npcInfoText.text = $"名字: {mainCharacter.name}\n" +
                              $"金币: {mainCharacter.coinValue}\n" +
                              $"体力值: {mainCharacter.brwanValue}";
        }
    }
    public void UpdateDogStats(string newEmotion, int newHealth, int newBelieve, int newEnergy)
    {
        myDog.emotion = newEmotion;
        myDog.healthValue = newHealth;
        myDog.believeValue = newBelieve;
        myDog.energyValue = newEnergy;

        UpdateCharacterUI();
    }

    // 更新NPC属性示例方法
    public void UpdateNPCStats(int newCoin, int newBrwan)
    {
        mainCharacter.coinValue = newCoin;
        mainCharacter.brwanValue = newBrwan;

        UpdateCharacterUI();
    }
}

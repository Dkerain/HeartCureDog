using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateUI : MonoBehaviour
{
    // 使用你现有的类体系
    public Dogs myDog;
    public NPCs mainCharacter;

    // 需要补充的UI更新方法
    private void UpdateCharacterUI()
    {
        // 实现你的UI更新逻辑，例如：
        // healthText.text = myDog.healthValue.ToString();
        Debug.Log("UI已更新");
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

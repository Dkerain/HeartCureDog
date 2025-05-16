using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateUI : MonoBehaviour
{
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

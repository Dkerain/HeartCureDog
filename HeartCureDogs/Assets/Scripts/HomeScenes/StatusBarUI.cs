using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusBarUI : MonoBehaviour
{
    public TextMeshProUGUI playerEnergyText;
    public TextMeshProUGUI playerCoinsText;
    public TextMeshProUGUI dogHealthText;
    public TextMeshProUGUI dogEnergyText;
    public TextMeshProUGUI dogTrustText;
    public TextMeshProUGUI dogMoodText;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (playerEnergyText != null)
            playerEnergyText.text = "体力值: " + PlayerStatus.Instance.playerEnergy;
        if (playerCoinsText != null)
            playerCoinsText.text = "金币: " + PlayerStatus.Instance.playerCoins;
        if (dogHealthText != null)
            dogHealthText.text = "体魄值: " + PlayerStatus.Instance.dogHealth;
        if (dogEnergyText != null)
            dogEnergyText.text = "精力值: " + PlayerStatus.Instance.dogEnergy;
        if (dogTrustText != null)
            dogTrustText.text = "信任值: " + PlayerStatus.Instance.dogTrust;
        if (dogMoodText != null)
            dogMoodText.text = "心情: " + PlayerStatus.Instance.dogMood;
    }
}
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NodeCanvas.Framework;

public class StatusBarUI : MonoBehaviour
{
    [Header("UI�ı����")]
    public TextMeshProUGUI playerEnergyText;  // �������ֵ
    public TextMeshProUGUI playerCoinsText;   // ��ҽ��
    public TextMeshProUGUI dogHealthText;     // С������ֵ
    public TextMeshProUGUI dogEnergyText;     // С������ֵ
    public TextMeshProUGUI dogTrustText;      // С������ֵ
    public TextMeshProUGUI dogMoodText;       // С������

    private GlobalBlackboard globalBlackboard;
    private bool blackboardFound = false;

    private void Start()
    {
        Debug.Log("UI�ű�����");
        FindGlobalBlackboard();
        UpdateUI();
    }

    private void FindGlobalBlackboard()
    {
        globalBlackboard = GlobalBlackboard.Find("Global");

        if (globalBlackboard != null)
        {
            blackboardFound = true;
            Debug.Log("UI�ɹ��ҵ�ȫ�ֺڰ壡");

            // ���Զ�ȡֵ
            try
            {
                Debug.Log($"���Զ�ȡ - ���: {globalBlackboard.GetValue<int>("npcCoinValue")}");
                Debug.Log($"���Զ�ȡ - ����: {globalBlackboard.GetValue<int>("npcBrwanValue")}");
                Debug.Log($"���Զ�ȡ - ����: {globalBlackboard.GetValue<int>("dogEnergyValue")}");
                Debug.Log($"���Զ�ȡ - ����: {globalBlackboard.GetValue<int>("dogHealthValue")}");
                Debug.Log($"���Զ�ȡ - ����: {globalBlackboard.GetValue<int>("dogBelieveValue")}");
                Debug.Log($"���Զ�ȡ - ����: {globalBlackboard.GetValue<string>("dogEmotion")}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"��ȡȫ�ֺڰ�ֵʱ����: {e.Message}");
            }
        }
        else
        {
            blackboardFound = false;
            Debug.LogError("UIδ�ҵ�ȫ�ֺڰ壡��ȷ���������б�ʶ��Ϊ'Global'��GlobalBlackboard��");
        }
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (!blackboardFound || globalBlackboard == null)
        {
            DisplayDefaultValues();
            return;
        }

        try
        {
            // ��ȫ�ֺڰ��ȡֵ������UI
            if (playerEnergyText != null)
            {
                int value = globalBlackboard.GetValue<int>("npcBrwanValue");
                playerEnergyText.text = "����ֵ: " + value;
                Debug.Log($"��������ֵUI: {value}");
            }

            if (playerCoinsText != null)
            {
                int value = globalBlackboard.GetValue<int>("npcCoinValue");
                playerCoinsText.text = "���: " + value;
                Debug.Log($"���½��UI: {value}");
            }

            if (dogHealthText != null)
            {
                int value = globalBlackboard.GetValue<int>("dogHealthValue");
                dogHealthText.text = "����ֵ: " + value;
                Debug.Log($"���½���ֵUI: {value}");
            }

            if (dogEnergyText != null)
            {
                int value = globalBlackboard.GetValue<int>("dogEnergyValue");
                dogEnergyText.text = "����ֵ: " + value;
                Debug.Log($"���¾���ֵUI: {value}");
            }

            if (dogTrustText != null)
            {
                int value = globalBlackboard.GetValue<int>("dogBelieveValue");
                dogTrustText.text = "����ֵ: " + value;
                Debug.Log($"��������ֵUI: {value}");
            }

            if (dogMoodText != null)
            {
                string value = globalBlackboard.GetValue<string>("dogEmotion");
                dogMoodText.text = "����: " + value;
                Debug.Log($"��������UI: {value}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("����UIʱ����: " + e.Message);
            DisplayDefaultValues();
        }
    }

    private void DisplayDefaultValues()
    {
        Debug.Log("��ʾĬ��ֵ");

        // ���Ҳ����ڰ�ʱ��ʾĬ��ֵ
        if (playerEnergyText != null)
            playerEnergyText.text = "����ֵ: 45";

        if (playerCoinsText != null)
            playerCoinsText.text = "���: 50";

        if (dogHealthText != null)
            dogHealthText.text = "����ֵ: 50";

        if (dogEnergyText != null)
            dogEnergyText.text = "����ֵ: 50";

        if (dogTrustText != null)
            dogTrustText.text = "����ֵ: 50";

        if (dogMoodText != null)
            dogMoodText.text = "����: ƽ��";
    }

    public void RefreshUI()
    {
        Debug.Log("�ֶ�ˢ��UI");
        UpdateUI();
    }

    private void OnEnable()
    {
        Debug.Log("UI�������");
        UpdateUI();
    }
}*/
using UnityEngine;
using TMPro;
using NodeCanvas.Framework;

public class StatusBarUI : MonoBehaviour
{
    public TextMeshProUGUI playerEnergyText;
    public TextMeshProUGUI playerCoinsText;
    public TextMeshProUGUI dogHealthText;
    public TextMeshProUGUI dogEnergyText;
    public TextMeshProUGUI dogTrustText;
    public TextMeshProUGUI dogMoodText;

    private Blackboard globalBlackboard;

    private void Start()
    {
        globalBlackboard = GlobalBlackboard.Find("Global");

        if (globalBlackboard == null)
        {
            Debug.LogError("δ�ҵ�ȫ�ֺڰ壡");
            return;
        }

        // ��ʼ����
        UpdateAllUI();
    }

    private void Update()
    {
        // ÿ֡���£��򵥵�Ч�ʽϵͣ�
        UpdateAllUI();
    }

    private void UpdateAllUI()
    {
        if (globalBlackboard == null) return;

        try
        {
            if (playerEnergyText != null)
                playerEnergyText.text = "����ֵ: " + globalBlackboard.GetVariableValue<int>("npcBrwanValue");
            if (playerCoinsText != null)
                playerCoinsText.text = "���: " + globalBlackboard.GetVariableValue<int>("npcCoinValue");
            if (dogHealthText != null)
                dogHealthText.text = "����ֵ: " + globalBlackboard.GetVariableValue<int>("dogHealthValue");
            if (dogEnergyText != null)
                dogEnergyText.text = "����ֵ: " + globalBlackboard.GetVariableValue<int>("dogEnergyValue");
            if (dogTrustText != null)
                dogTrustText.text = "����ֵ: " + globalBlackboard.GetVariableValue<int>("dogBelieveValue");
            if (dogMoodText != null)
                dogMoodText.text = "����: " + globalBlackboard.GetVariableValue<string>("dogEmotion");
        }
        catch (System.Exception e)
        {
            Debug.LogError("����UIʱ����: " + e.Message);
        }
    }
}
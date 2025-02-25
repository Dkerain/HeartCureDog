using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSelectionManager : MonoBehaviour
{
    public static DogSelectionManager Instance { get; private set; }

    public int selectedDogType = 0; // 默认选择第一种小狗
    public string selectedMBTI = "INFJ"; // 默认选择一种MBTI类型

    public GameObject dogSelectionPanel; // 在 Inspector 中分配
    public GameObject mbtiSelectionPanel; // 在 Inspector 中分配

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 保留实例
        }
        else
        {
            Destroy(gameObject); // 销毁重复实例
        }
    }

    public void SelectDog(int type)
    {
        selectedDogType = type;
        Debug.Log("Selected Dog Type: " + selectedDogType);
    }

    public void SelectMBTI(string mbti)
    {
        selectedMBTI = mbti;
        Debug.Log("Selected MBTI: " + selectedMBTI);
    }

    public void ShowMBTISelection()
    {
        if (dogSelectionPanel != null && mbtiSelectionPanel != null)
        {
            dogSelectionPanel.SetActive(false);
            mbtiSelectionPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Panels are not assigned correctly in the Inspector!");
        }
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene"); // 加载游戏场景
    }

    public void TestButton()
    {
        Debug.Log("Button clicked!");
    }
}

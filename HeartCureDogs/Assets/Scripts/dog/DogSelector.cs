using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DogSelector : MonoBehaviour
{
    public Button[] selectButtons;    // 三个选择按钮，顺序对应品种索引
    public string nextSceneName = "GameScene"; // 选择后跳转的场景名称

    void Start()
    {
        for (int i = 0; i < selectButtons.Length; i++)
        {
            int index = i; // 局部变量捕获
            selectButtons[i].onClick.AddListener(() => SelectDog(index));
        }
    }

    private void SelectDog(int breedIndex)
    {
        // 保存玩家选择的品种索引
        PlayerPrefs.SetInt("SelectedBreed", breedIndex);
        PlayerPrefs.Save();

        Debug.Log($"选择了小狗品种索引: {breedIndex}");

        // 跳转到游戏主场景
        SceneManager.LoadScene(nextSceneName);
    }
}
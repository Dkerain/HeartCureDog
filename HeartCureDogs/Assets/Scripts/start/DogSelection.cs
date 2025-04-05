using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DogSelection : MonoBehaviour
{
    public enum DogBreed { Labrador, Corgi, Husky }

    [Header("UI引用")]
    public GameObject confirmButton; // 需要在Inspector中拖入确认按钮
    public TextMeshProUGUI infoText;

    [Header("对话控制")]
    public CombinedDialogueSystem dialogueSystem; // 需要在Inspector中拖入对话系统组件

    [Header("品种介绍")]
    public string[] breedDescriptions = new string[]
    {
        "拉布拉多犬，性格温顺友善，对人类和其他动物都非常友好，是孩子们的最佳玩伴。聪明易训，学习能力强。",
        "柯基，性格开朗活泼，聪明机警，对主人忠诚，对陌生人保持警惕。喜欢玩耍和运动。",
        "哈士奇，性格活泼好动，精力充沛，喜欢奔跑和探索。聪明机警，但同时也有些固执和独立。"
    };

    private DogBreed selectedBreed;
    private bool hasSelected = false;

    void Start()
    {
        confirmButton.SetActive(false); // 初始隐藏确认按钮
        StartIntroduction();
    }

    void StartIntroduction()
    {
        // 触发介绍对话，从第0行到第6行
        dialogueSystem.LoadDialogueData("0", "6");
    }

    // 当点击品种按钮时调用（需在按钮事件中关联）
    public void SelectDog(int breedIndex)
    {
        if (hasSelected) return;

        selectedBreed = (DogBreed)breedIndex;
        confirmButton.SetActive(true); // 显示确认按钮
        infoText.text = $"当前选择：{selectedBreed}\n{breedDescriptions[breedIndex]}";
    }

    // 确认按钮点击事件
    public void ConfirmSelection()
    {
        hasSelected = true;
        confirmButton.SetActive(false); // 隐藏确认按钮
        StartCoroutine(FinalProcess());
    }

    IEnumerator FinalProcess()
    {
        // 保存选择的品种
        PlayerPrefs.SetInt("SelectedBreed", (int)selectedBreed);

        // 触发告别对话
        dialogueSystem.LoadDialogueData("7", "9"); // 加载对话文件中的第5行到第6行

        // 等待对话结束
        while (dialogueSystem.IsDialogueActive())
            yield return null;

        // 延迟0.5秒后切换场景
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("HomeScene");
    }
}
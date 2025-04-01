using System.Collections;
using System.Collections.Generic;
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
    public DialogueSystem dialogueSystem; // 需要在Inspector拖入对话系统组件
    public List<string> farewellDialog = new List<string>
    {
        "印小棠：祝您和狗狗相处愉快！",
        "如果有任何问题，随时欢迎您来店里。"
    };

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
        dialogueSystem.StartDialogue(new List<string>
        {
            "欢迎光临！",
            "这是我们店里最受欢迎的三只小狗。让我为您介绍一下！",
            "左边这只是拉布拉多犬，拉布拉多可是出了名的“好好先生”！它们性格温顺友善，对人类和其他动物都非常友好，是孩子们的最佳玩伴。它们智商排名犬类第七，聪明易训，学习能力强，是优秀的工作犬和家庭伴侣犬。拉布拉多精力充沛，活泼好动，喜欢玩耍和运动，每天都需要充足的运动量来保持身心健康。最重要的是，它们对主人忠诚可靠，性格稳定，是值得信赖的家庭成员。",
            "中间这只是柯基。别看柯基腿短，它们可是精力充沛的小家伙！性格开朗活泼，聪明机警，对主人忠诚，对陌生人保持警惕。柯基学习能力强，容易训练，但也有些固执，需要主人有耐心和技巧。它们喜欢玩耍和运动，也喜欢和家人互动，是很好的家庭伴侣犬。",
            "右边这只是哈士奇，哈士奇可是出了名的“二哈”！它们性格活泼好动，精力充沛，喜欢奔跑和探索，是出了名的“撒手没”。哈士奇聪明机警，但同时也有些固执和独立，训练起来需要更多的耐心和技巧。它们对主人忠诚，但对陌生人也很友好，是典型的“自来熟”。",
            "挑选一只你想饲养的宠物吧！"
        });
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

        // 显示告别对话
        dialogueSystem.StartDialogue(farewellDialog);

        // 等待对话结束
        while (dialogueSystem.IsDialogueActive())
            yield return null;

        // 延迟0.5秒后切换场景
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("HomeScene");
    }
}
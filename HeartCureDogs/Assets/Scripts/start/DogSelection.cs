using System.Collections;
using System.Collections.Generic;
// DogSelection.cs
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DogSelection : MonoBehaviour
{
    public enum DogBreed { Labrador, Corgi, Husky }

    [Header("UI引用")]
    public GameObject confirmButton;
    public TextMeshProUGUI infoText;

    [Header("品种介绍")]
    public string[] breedDescriptions;

    private DogBreed selectedBreed;

    void Start()
    {
        StartIntroduction();
    }

    void StartIntroduction()
    {
        GetComponent<DialogueSystem>().StartDialogue(new List<string>{
            "欢迎光临！",
            "这是我们店里最受欢迎的三只小狗。让我为您介绍一下！",
            "左边这只是拉布拉多犬，拉布拉多可是出了名的“好好先生”！它们性格温顺友善，对人类和其他动物都非常友好，是孩子们的最佳玩伴。它们智商排名犬类第七，聪明易训，学习能力强，是优秀的工作犬和家庭伴侣犬。拉布拉多精力充沛，活泼好动，喜欢玩耍和运动，每天都需要充足的运动量来保持身心健康。最重要的是，它们对主人忠诚可靠，性格稳定，是值得信赖的家庭成员。",
            "中间这只是柯基。别看柯基腿短，它们可是精力充沛的小家伙！性格开朗活泼，聪明机警，对主人忠诚，对陌生人保持警惕。柯基学习能力强，容易训练，但也有些固执，需要主人有耐心和技巧。它们喜欢玩耍和运动，也喜欢和家人互动，是很好的家庭伴侣犬。",
            "右边这只是哈士奇，哈士奇可是出了名的“二哈”！它们性格活泼好动，精力充沛，喜欢奔跑和探索，是出了名的“撒手没”。哈士奇聪明机警，但同时也有些固执和独立，训练起来需要更多的耐心和技巧。它们对主人忠诚，但对陌生人也很友好，是典型的“自来熟”",
            "挑选一只你想饲养的宠物吧！"
        });
    }

    public void SelectDog(int breedIndex)
    {
        selectedBreed = (DogBreed)breedIndex;
        confirmButton.SetActive(true);
        infoText.text = $"当前选择：{selectedBreed}\n{breedDescriptions[breedIndex]}";
    }

    public void ConfirmSelection()
    {
        PlayerPrefs.SetInt("SelectedBreed", (int)selectedBreed);
        SceneManager.LoadScene("HomeScene");
    }
}

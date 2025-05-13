using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class homesceneson : MonoBehaviour
{
    public GameObject[] dogPrefabs;
    public Transform spawnPoint;
    public Transform spawnPoint1;
    public GameObject blanket;
    public GameObject statusPanel;
    public GameObject feedPrompt;
    public GameObject foodBowl;
    public GameObject dialogueBox;
    [Header("对话系统")]
    public DialogueSystem dialogueSystem;
    public GameObject leaveButton;
    public GameObject optionsPanel;

    private DogControllerr dogController; // DogController 的引用
    private GameObject dogInstance;
    private bool hasFedDog = false;

    private void Start()
    {
        if (feedPrompt == null || dialogueBox == null || statusPanel == null)
        {
            Debug.LogError("某些 UI 组件未正确绑定！");
        }

        statusPanel.SetActive(true);

        // 初始化新版控制器
        InitializeDogController();

        dialogueBox.SetActive(false);
        feedPrompt.SetActive(true);
        StartCoroutine(FeedDogRoutine());
    }
    private void InitializeDogController()
    {
        dogController = FindObjectOfType<DogControllerr>();

        if (dogController != null)
        {
            // 如果需要在初始化时锁定位置
            dogController.GetComponent<DogControllerr>().enabled = false;
        }
        else
        {
            Debug.LogError("找不到DogControllerr，请确保场景中有对应的控制器！");
        }
    }

    private IEnumerator FeedDogRoutine()
    {
        yield return new WaitForSeconds(1f);
        feedPrompt.SetActive(true);
        feedPrompt.GetComponentInChildren<TextMeshProUGUI>().text = "狗狗的喂餐时间到~\n点击饭盆给狗狗喂食吧！";

        // 等待点击饭盆
        while (!Input.GetMouseButtonDown(0) && !hasFedDog)
        {
            yield return null;
        }

        if (!hasFedDog)
        {
            feedPrompt.SetActive(false);
            foodBowl.SetActive(true);

            Vector3 targetPosition = spawnPoint.position + new Vector3(0, 0, -1);
            float elapsedTime = 0f;
            float duration = 1f;

            while (elapsedTime < duration)
            {
                foodBowl.transform.position = Vector3.Lerp(foodBowl.transform.position, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            foodBowl.transform.position = targetPosition;
            yield return new WaitForSeconds(2f);

            feedPrompt.SetActive(true);
            feedPrompt.GetComponentInChildren<TextMeshProUGUI>().text = "给饭盆添加食物吧！";
            hasFedDog = true;
        }
    }

    public void OnAddFoodButtonClicked()
    {
        feedPrompt.SetActive(false);
        dialogueBox.SetActive(true);

        // 添加安全检查
        if (foodBowl.TryGetComponent<Button>(out var bowlButton))
        {
            bowlButton.interactable = false;
        }

        if (dialogueSystem == null)
        {
            Debug.LogError("对话系统实例未初始化！");
            return;
        }

        StartCoroutine(SafeStartDialogue());
    }

    private IEnumerator SafeStartDialogue()
    {
        // 更新对话启动方式
        if (dialogueSystem == null)
        {
            Debug.LogError("缺少对话系统引用！");
            yield break;
        }

        dialogueSystem.StartDialogue(new List<string>
        {
            "你：呀，小可爱，一到饭点就这么兴奋呢。",
            "系统：这次的喂食量比平时要少，此时你有急事马上要出门，正面临两个选择......"
        });

        yield return new WaitWhile(() => dialogueSystem.IsDialogueActive());
        StartCoroutine(ShowOptions());
    }

    private IEnumerator ShowOptions()
    {
        while (dialogueSystem.IsDialogueActive())
            yield return null;

        optionsPanel.SetActive(true);

        // 创建选项按钮的推荐方式
        CreateOptionButton("Option1", "1、快点去厨房再拿些狗粮给墩墩吧，待会路上快点就好了。", -150, Option1Selected);
        CreateOptionButton("Option2", "2、来不及了，墩墩今天少吃点算了。", 150, Option2Selected);
    }

    private void CreateOptionButton(string name, string text, float xPos, UnityEngine.Events.UnityAction callback)
    {
        GameObject option = new GameObject(name);
        var button = option.AddComponent<Button>();
        var textComponent = option.AddComponent<TextMeshProUGUI>();

        textComponent.text = text;
        textComponent.fontSize = 24;
        textComponent.color = Color.white;

        button.onClick.AddListener(callback);
        option.transform.SetParent(optionsPanel.transform);
        option.GetComponent<RectTransform>().localPosition = new Vector3(xPos, 0, 0);
    }

    void Option1Selected()
    {
        ClearOptions();
        dialogueSystem.StartDialogue(new List<string> { "充足的食物对幼犬期的墩墩的成长非常重要，您的选择正确！" });
        StartCoroutine(UpdatePlayerStatus(3));
    }

    void Option2Selected()
    {
        ClearOptions();
        dialogueSystem.StartDialogue(new List<string> { "充足的食物对幼犬期的墩墩的成长非常重要，您的选择错误。" });
        StartCoroutine(UpdatePlayerStatus(-3));
    }

    private void ClearOptions()
    {
        optionsPanel.SetActive(false);
        foreach (Transform child in optionsPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator UpdatePlayerStatus(int amount)
    {
        yield return new WaitWhile(() => dialogueSystem.IsDialogueActive());
        dialogueBox.SetActive(false);
        if (PlayerStatus.Instance != null)
        {
            PlayerStatus.Instance.playerEnergy += amount;
            leaveButton.SetActive(true);
        }
    }

    public void OnLeaveButtonClicked()
    {
        leaveButton.SetActive(false);
        StartCoroutine(AfterLeaveActions());
    }

    private IEnumerator AfterLeaveActions()
    {
        yield return new WaitForSeconds(0.5f);

        // 启用新版移动控制
        if (dogController != null)
        {
            dogController.enabled = true;
            // 设置一个预设的初始目标点 （示例位置）
            Vector3 initialTarget = blanket.transform.position + new Vector3(2f, 0, 0);

            // 调用新版方法
            dogController.SetTargetPosition(initialTarget);
        }
        else
        {
            Debug.LogError("DogControllerr 组件未正确绑定！");
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Animator dogAnimator; // 小狗的动画控制器
    public GameObject foodBowl; // 饭盆的GameObject
    public GameObject hintPanel; // 提示框的GameObject
    public Text hintText; // 提示文本
    public GameObject choicePanel; // 选项框的GameObject
    public DialogueManager dialogueManager; // 对话管理器
    public StatusManager statusManager; // 状态管理器

    private void Start()
    {
        // 初始化状态栏
        statusManager.UpdateStatus();

        // 初始化对话管理器，但一开始隐藏对话框
        dialogueManager.dialogues = new string[]
        {
            "你：呀，一到饭点就这么兴奋呢！",
            "你还想继续倒时发现狗粮袋已经空了，此时你看了眼表，上班快要迟到了，你面临两个选择......"
        };
        dialogueManager.gameObject.SetActive(false); // 隐藏对话框

        // 初始化小狗动画
        dogAnimator.Play("Idle");

        // 5秒后显示小狗
        Invoke("ShowDog", 2f);
    }

    private void ShowDog()
    {
        // 3秒后小狗睁开眼睛
        Invoke("WakeUpDog", 2f);
    }

    private void WakeUpDog()
    {
        dogAnimator.SetTrigger("WakeUpTrigger"); // 触发小狗睁开眼睛的动画
        ShowHint("点击饭盆给狗狗喂食"); // 显示提示框
    }

    public void FeedDog()
    {
        // 触发小狗被喂食的动画
        dogAnimator.SetTrigger("FeedTrigger");
        Debug.Log("触发 FeedTrigger 动画");

        // 在 FeedTrigger 动画结束后触发 EatTrigger
        Invoke("TriggerEat", 2f); // 假设 FeedTrigger 动画持续时间为 2 秒

        // 5秒后显示对话框
        Invoke("ShowDialogue", 2f);
        HideHint(); // 隐藏提示框
    }

    private void TriggerEat()
    {
        dogAnimator.SetTrigger("EatTrigger"); // 触发小狗吃饭的动画
        Debug.Log("触发 EatTrigger 动画");
    }

    private void ShowDialogue()
    {
        dialogueManager.ShowDialogue(dialogueManager.dialogues[0]); // 显示第一部分对话
        dialogueManager.gameObject.SetActive(true); // 显示对话框
        Invoke("ShowDialoguePart2", 3f); // 5秒后显示第二部分对话
    }

    private void ShowDialoguePart2()
    {
        dialogueManager.ShowDialogue(dialogueManager.dialogues[1]); // 显示第二部分对话
        Invoke("ShowChoices", 3f); // 5秒后显示对话选项
    }

    private void ShowHint(string hint)
    {
        hintText.text = hint; // 设置提示文本
        hintPanel.SetActive(true); // 显示提示框
    }

    private void HideHint()
    {
        hintPanel.SetActive(false); // 隐藏提示框
    }

    private void ShowChoices()
    {
        choicePanel.SetActive(true); // 显示选项框
    }

    private void HideChoices()
    {
        choicePanel.SetActive(false); // 隐藏选项框
    }

    public void Choice1()
    {
        dialogueManager.ShowDialogue("你拿着新的一袋狗粮出现，狗狗看着饭盆里多出的狗粮，发出“呜呜”声，尾巴摇来摇去的，心里开心坏了。");
        Invoke("FeedDogAgain", 3f); // 3秒后再次触发喂食动画
        HideChoices(); // 玩家选择后立即隐藏选项框
    }

    private void FeedDogAgain()
    {
        dogAnimator.SetTrigger("FeedTrigger"); // 再次触发小狗被喂食的动画
        Debug.Log("再次触发 FeedTrigger 动画");

        // 在 FeedTrigger 动画结束后触发 EatTrigger
        Invoke("TriggerEat", 2f); // 假设 FeedTrigger 动画持续时间为 2 秒

        dialogueManager.ShowDialogue("你：真羡慕你有这么好的主人，我走啦，在家乖乖的哦。");
        Invoke("UpdateStatus1", 1f); // 3秒后更新状态
    }

    private void UpdateStatus1()
    {
        statusManager.hunger = 100f; // 更新饱腹值
        statusManager.cleanliness = 100f; // 更新清洁值
        statusManager.health = 100f; // 更新健康值
        statusManager.energy = 100f; // 更新精力值
        statusManager.mood = 100f; // 更新心情值
        statusManager.UpdateStatus(); // 更新状态栏显示
        dialogueManager.gameObject.SetActive(false); // 隐藏对话框
    }

    public void Choice2()
    {
        dialogueManager.ShowDialogue("你离开了。");
        dogAnimator.SetTrigger("SadTrigger"); // 触发小狗感到悲伤的动画
        Debug.Log("触发 SadTrigger 动画");

        Invoke("UpdateStatus2", 1f); // 3秒后更新状态
        HideChoices(); // 玩家选择后立即隐藏选项框
    }

    private void UpdateStatus2()
    {
        statusManager.hunger = 80f; // 更新饱腹值
        statusManager.cleanliness = 100f; // 更新清洁值
        statusManager.health = 100f; // 更新健康值
        statusManager.energy = 100f; // 更新精力值
        statusManager.mood = 80f; // 更新心情值
        statusManager.UpdateStatus(); // 更新状态栏显示
        dialogueManager.gameObject.SetActive(false); // 隐藏对话框
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEngine;
using UnityEngine.UI;
using DogUnity;
using System.Linq;

public class TextInputHandler : MonoBehaviour
{
    [SerializeField]
    public TMP_InputField playerInputField;
    [SerializeField] private TMP_Text glmText;
    public Image DialogueImage;

    [SerializeField,Tooltip("玩家和GLM的历史对话记录以List<SendData>格式保存")]
    private List<SendData> chatHistory = new List<SendData>();
    // Update is called once per frame
    public void Start()
    {
        //创建system prompt，让GLM进行角色扮演。（并不强制要求）
        SendData systemPrompt = new SendData()
        {
            role="system",
            content="请扮演一只可爱的小狗，我是你的主人。"
        };
        // 将system prompt作为第一条对话记录加入chatHistory
        chatHistory.Add(systemPrompt);

        //DialogueImage = GameObject.Find("DialogueImage").GetComponent<Image>();

        //// 检查 targetImage 是否已赋值
        //if (DialogueImage == null)
        //{
        //    Debug.LogError("Target Image is not assigned!");
        //    return;
        //}

        //// 初始隐藏图片
        //DialogueImage.gameObject.SetActive(false);
    }
   public async void SendPlayerResponse()
   {
        // 从InputField读取玩家的输入
        string playerInput = playerInputField.text;
        playerInputField.text = "";

        // 创建玩家SendData信息，并将其加入chatHistory
        SendData playerMessage = new SendData()
        {
            role = "user",
            content = playerInput
        };
        chatHistory.Add(playerMessage);

        // 使用GLMHandler.GenerateGLMResponse生成GLM回复，设置tmeperature=0.8
        // 注意需要使用await关键词
        SendData respone = await DogHandler.GenerateDogResponse(chatHistory, 0.8f);
        // 将GLM的回复加进chatHistory
        chatHistory.Add(respone);

        // 使用response.content获取GLM的回复
        glmText.text = respone.content;
    }
    //public void ToggleModule()
    //{
    //    if (DialogueImage != null)
    //    {
    //        DialogueImage.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("Target Image is null!");
    //    }
    //}
}

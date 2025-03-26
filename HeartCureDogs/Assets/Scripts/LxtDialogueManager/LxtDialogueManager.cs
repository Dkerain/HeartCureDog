using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.UI;
using UnityEngine.UI;

public class LxtDialogueManager : MonoBehaviour
{
    public TextAsset dialoguedataFiletest;//对话文本文件，csv格式
    public TMP_Text rolenameText;//人物名字文本
    public TMP_Text roledialogueText;//人物对话文本
    public GameObject roleImage;//人物对话图片
    public List<Sprite> roleSprites = new List<Sprite>();//建立图片链表
    Dictionary<string, Sprite> rolespritesDic = new Dictionary<string, Sprite>();//建立名字和图片的字典
    public int dialogueIndex;//当前的对话索引值
    public string[] dialogueRows;

    private void Awake()
    {
        rolespritesDic["墩墩"] = roleSprites[0];
    }
    // Start is called before the first frame update
    void Start()
    {
        ReadText(dialoguedataFiletest);
        //UpdateText("墩墩", "你好");
        //UpdateSprite("墩墩");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateText(string _rolename, string _roledialogue)
    {
        rolenameText.text = _rolename;
        roledialogueText.text = _roledialogue;
    }
    public void UpdateSprite(string _rolename)
    {
        roleImage.GetComponent<Image>().sprite = rolespritesDic[_rolename];
    }
    public void ReadText(TextAsset _textassetname)
    {
        dialogueRows = _textassetname.text.Split('\n');//每一行都读取
        //foreach (var row in rows)
        //{
        //    string[] cell = row.Split(','); 
        //}
        Debug.Log("读取成功");
    }
    public void ShowDialogueRows()
    {
        foreach (var row in dialogueRows)
        {
            string[] cells = row.Split(',');
            if(int.Parse(cells[1]) == dialogueIndex && cells[0]=="#")
            {
                UpdateText(cells[2], cells[3]);
                UpdateSprite(cells[2]);
                dialogueIndex = int.Parse(cells[4]);
            }
        }
    }
}

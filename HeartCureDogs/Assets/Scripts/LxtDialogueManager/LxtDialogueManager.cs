using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.UI;
using UnityEngine.UI;
//using UnityEngine.TextCore.Text;
using System.IO.Pipes;

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
    public Button nextButton;
    public GameObject optionButton;//选项按钮预制体
    public Transform buttonGroup;//选项按钮父节点，用于自动排序
    public List<Character> chracters = new List<Character>();

    private void Awake()
    {
        rolespritesDic["印小棠"] = roleSprites[0];
        rolespritesDic["旁白"]=roleSprites[1];
        Character npc0 = new Character();
        npc0.name = "印小棠";
        chracters.Add(npc0);
        Character narration = new Character();
        narration.name = "旁白";
        chracters.Add(narration);
    }
    // Start is called before the first frame update
    void Start()
    {
        ReadText(dialoguedataFiletest);
        ShowDialogueRows();
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
        for (int i=0;i<dialogueRows.Length;i++)
        {
            string[] cells = dialogueRows[i].Split(',');
            if (cells[0] == "#" && int.Parse(cells[1]) == dialogueIndex)
            {
                //if (cells[2] == null)
                //{
                //    rolenameText.text = null;
                //    roleImage.GetComponent<Image>().sprite = null;
                //}
                UpdateText(cells[2], cells[3]);
                UpdateSprite(cells[2]);
                dialogueIndex = int.Parse(cells[4]);
                nextButton.gameObject.SetActive(true);
                break;
            }
            else if (cells[0] == "&" && int.Parse(cells[1])==dialogueIndex)
            {
                nextButton.gameObject.SetActive(false);
                GenerateOption(i);
            }
            else if (cells[0] == "END" && int.Parse(cells[1]) == dialogueIndex)
            {
                Debug.Log("剧情结束");
            }
        }
    }
    public void OnClickNextButton()
    {
        ShowDialogueRows();
    }
    public void GenerateOption(int _index)
    {
        string[] cells=dialogueRows[_index].Split(",");
        if (cells[0] == "&")
        {
            GameObject button = Instantiate(optionButton, buttonGroup);
            button.GetComponentInChildren<TMP_Text>().text = cells[4];
            button.GetComponent<Button>().onClick.AddListener
                (
                    delegate
                    {
                        OnOptionClick(int.Parse(cells[5]));
                        if (cells[6] != " ")
                        {
                            string[] effect = cells[6].Split("@");
                            OptionEffect(effect[0], int.Parse(effect[1]), cells[7]);
                        }
                    }
                );
            GenerateOption(_index + 1);
        }
    }
    public void OnOptionClick(int _id)
    {
        dialogueIndex = _id;
        ShowDialogueRows();
        for (int i = 0; i < buttonGroup.childCount; i++)
        {
            Destroy(buttonGroup.GetChild(i).gameObject);
        }
    }
    public void OptionEffect(string _effect, int _param, string _target)
    {
        if (_effect == "体力值加")
        {
            foreach (var character in chracters)
            {
                if (character.name == _target)
                {
                    character.brwanValue += _param;
                }
            }
        }
        if (_effect == "体力值减")
        {
            foreach (var character in chracters)
            {
                if (character.name == _target)
                {
                    character.brwanValue -= _param;
                }
            }
        }
        if (_effect == "金币加")
        {
            foreach (var character in chracters)
            {
                if (character.name == _target)
                {
                    character.coinValue -= _param;
                }
            }
        }
        if (_effect == "金币减")
        {
            foreach (var character in chracters)
            {
                if (character.name == _target)
                {
                    character.coinValue -= _param;
                }
            }
        }
        if (_effect == "体魄减")
        {
            foreach (var character in chracters)
            {
                if (character.name == _target)
                {
                    character.healthValue -= _param;
                }
            }
        }
        if (_effect == "体魄加")
        {
            foreach (var character in chracters)
            {
                if (character.name == _target)
                {
                    character.healthValue -= _param;
                }
            }
        }
        if (_effect == "精力加")
        {
            foreach (var character in chracters)
            {
                if (character.name == _target)
                {
                    character.energyValue += _param;
                }
            }
        }
        if (_effect == "精力减")
        {
            foreach (var character in chracters)
            {
                if (character.name == _target)
                {
                    character.energyValue -= _param;
                }
            }
        }
        if (_effect == "信任加")
        {
            foreach (var character in chracters)
            {
                if (character.name == _target)
                {
                    character.believeValue += _param;
                }
            }
        }
        if (_effect == "信任减")
        {
            foreach (var character in chracters)
            {
                if (character.name == _target)
                {
                    character.believeValue -= _param;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.UI;
using UnityEngine.UI;

public class LxtDialogueManager : MonoBehaviour
{
    public TextAsset dialoguedatafiletest;//对话文本文件，csv格式
    public TMP_Text rolenametext;
    public TMP_Text roledialoguetext;
    public List<Sprite> rolesprites=new List<Sprite>();
    Dictionary<string,Sprite> rolespritesdic = new Dictionary<string,Sprite>();

    private void Awake()
    {
        rolespritesdic[""]
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

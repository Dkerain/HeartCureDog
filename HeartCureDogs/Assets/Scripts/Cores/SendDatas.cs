using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//public class SendDatas : MonoBehaviour
//{
namespace DogUnity
{
    [System.Serializable]
    public class SendData
    {
        [SerializeField] public string role;
        [SerializeField] public string content;
        public List<DogFunctionsTools> tool_calls;
        public SendData() { }
        public SendData(string _role, string _content)
        {
            role = _role;
            content = _content;
        }
        public List<string> DataList { get; set; }
        /*public SendData()
        {
            DataList = new List<string>();
        }*/
        /*public static class SendDataExtensions
        {
            public static bool Contains(this SendData sendData, object item)
            {
                // 实现 Contains 的逻辑
                // 例如：
                // return sendData.YourCollection.Contains(item);
            }
        }*/
    }
}
    // Start is called before the first frame update
    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
//}

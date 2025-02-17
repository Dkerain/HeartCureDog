using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DogUnity
{
    /// <summary>
    /// GLM工具基类
    /// GLM目前可以调用：function, retrieval, web_search
    /// </summary>
    public abstract class DogTools
    {
        public virtual string type { get; }
    }
}
/*public class DogTools : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}*/

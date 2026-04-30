using UnityEngine;
using System.Collections.Generic;

public class PropManager : MonoBehaviour
{
    [System.Serializable]
    public class Prop
    {
        public string propName;      // 物品名称，如 "玩具球"
        public GameObject propObject; // 场景中的物品对象
    }

    public List<Prop> props;

    // 显示指定物品
    public void ShowProp(string propName)
    {
        var prop = props.Find(p => p.propName == propName);
        if (prop != null && prop.propObject != null)
        {
            prop.propObject.SetActive(true);
            Debug.Log($"物品 [{propName}] 已显示");
        }
        else
        {
            Debug.LogWarning($"未找到名为 '{propName}' 的物品");
        }
    }

    // 隐藏指定物品
    public void HideProp(string propName)
    {
        var prop = props.Find(p => p.propName == propName);
        if (prop != null && prop.propObject != null)
        {
            prop.propObject.SetActive(false);
            Debug.Log($"物品 [{propName}] 已隐藏");
        }
        else
        {
            Debug.LogWarning($"未找到名为 '{propName}' 的物品");
        }
    }

    // 显示所有物品（可选）
    public void ShowAllProps()
    {
        foreach (var prop in props)
        {
            if (prop.propObject != null)
                prop.propObject.SetActive(true);
        }
    }

    // 隐藏所有物品（可选）
    public void HideAllProps()
    {
        foreach (var prop in props)
        {
            if (prop.propObject != null)
                prop.propObject.SetActive(false);
        }
    }
}
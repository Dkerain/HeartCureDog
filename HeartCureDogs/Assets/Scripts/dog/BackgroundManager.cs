using UnityEngine;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour
{
    [System.Serializable]
    public class BackgroundEntry
    {
        public string name;          // 背景名称，例如 "客厅"
        public GameObject bgObject;  // 对应的背景物体（Panel或Canvas）
    }

    public List<BackgroundEntry> backgrounds;
    private GameObject currentActiveBg;

    public void SwitchTo(string bgName)
    {
        BackgroundEntry target = backgrounds.Find(b => b.name == bgName);
        if (target == null || target.bgObject == null)
        {
            Debug.LogWarning($"背景管理器：找不到名为 '{bgName}' 的背景");
            return;
        }

        if (currentActiveBg != null)
            currentActiveBg.SetActive(false);

        target.bgObject.SetActive(true);
        currentActiveBg = target.bgObject;
    }

    public void HideAllBackgrounds()
    {
        foreach (var bg in backgrounds)
        {
            if (bg.bgObject != null)
                bg.bgObject.SetActive(false);
        }
        currentActiveBg = null;
    }
}
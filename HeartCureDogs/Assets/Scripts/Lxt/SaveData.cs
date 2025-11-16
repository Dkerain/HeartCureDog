using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int Chapter;
    public string Title;
    public DateTime SaveTime;
    public string SaveId;
    public string SceneName;

    public SaveData()
    {
        SaveId = Guid.NewGuid().ToString();
        SaveTime = DateTime.Now;
    }
}

[Serializable]
public class SaveGameCollection
{
    public List<SaveData> SaveSlots = new List<SaveData>();

    // 确保至少有3个存档位
    public SaveGameCollection()
    {
        while (SaveSlots.Count < 3)
        {
            SaveSlots.Add(new SaveData());
        }
    }
}
